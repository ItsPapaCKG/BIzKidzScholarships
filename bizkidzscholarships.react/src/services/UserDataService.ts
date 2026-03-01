import { type ITask, type ITaskJSON, type StartUploadRequest, ActionType, type UploadHandshakeConfirmation, RequestStatus, type StartUploadHandshakeResponseJSON, type ServerUploadResponse, type UserPointsJSON, type TaskQuizAnswers, type TaskQuestion, type BizDocumentType, type ConsentResponse } from "../models/ViewModels";
import { APICall, type APIResponse } from "./APIService";

export async function CheckUserProfile(): Promise<boolean> {
    return false;
}

export async function GetUserTasks(): Promise<ITask[]> {
    var res = await fetch(`/api/user/tasks`, {
        credentials: "include"
    });

    var jsonResult = await res.json();

    var tasksJSON = jsonResult as ITaskJSON[];

    var tasks: ITask[] = [];

    tasksJSON.forEach((i) => {
        tasks.push({
            TaskTitle: i.taskTitle,
            TaskDescription: i.taskDescription,
            Reward: i.reward,
            Status: i.status,
            TaskImageKey: i.taskImageKey,
            TaskId: i.taskId,
            TaskType: i.taskType,
            TaskPromptTitle: i.taskPromptTitle,
            TaskPromptSubtitle: i.taskPromptSubtitle
        } as ITask)
    });

    return tasks;
}

export async function TaskUpload(taskid: Number, file: File, consentId: number): Promise<boolean> {
    let request = {
        ActionType: ActionType.TaskUpload,
        Extension: file.name.split(".").pop()!.toLowerCase(),
        TaskId: taskid,
        IsPrivate: true,
        ConsentId: consentId ?? 0
    } as StartUploadRequest

    return (await UploadToServer(request, file)).Success;
}

export async function ProfileUpload(file: File) {
    let request = {
        ActionType: ActionType.ProfileImageUpload,
        Extension: file.name.split(".").pop()!.toLowerCase(),
        IsPrivate: false,
        ConsentId: 0
    } as StartUploadRequest

    return await UploadToServer(request, file);
}

async function GetPresignedS3Url(request: StartUploadRequest)  {

    var res = await APICall<StartUploadHandshakeResponseJSON>("user/NewUploadRequest", "POST", request);

    if (res.success) {
        return res.data!;
    }

    return null;
}

async function UploadToServer(request: StartUploadRequest, file: File): Promise<ServerUploadResponse> {
    let presignedDataResponse: StartUploadHandshakeResponseJSON | null = await GetPresignedS3Url(request);
    let result = { Success: false } as ServerUploadResponse

    if (presignedDataResponse == null || presignedDataResponse.presignedUrlPayload == null) {
        console.error("An internal error has occurred: No Presigned URL could be read from the server.");
        return result;
    }

    let presignedData = presignedDataResponse.presignedUrlPayload;

    result.Url = presignedData.url + presignedData.key;

    var formdata = new FormData();
    Object.entries(presignedData.fields).forEach(([k, v]) => {
        formdata.append(k, v);
    });

    formdata.append("file",file);

    var upload = await fetch(presignedData.url, {
        method: "POST",
        body: formdata
    })

    if (upload.ok) {
        // alert("Upload successful! See: " + presignedData.url + presignedData.key) 
        var success = await CompleteUploadHandshake(presignedDataResponse.requestId, RequestStatus.Success);

        result.Success = success;
        return result;
    }

    let responseText = await upload.text().catch(() => "(no body)");

    console.log(responseText);
    alert("Upload failed.");

    var success = await CompleteUploadHandshake(presignedDataResponse.requestId, RequestStatus.Failed);

    result.Success = success;
    return result;
}

async function CompleteUploadHandshake(requestId: string, requestStatus: RequestStatus) {
    var confirmation = {
        RequestId: requestId,
        Status: requestStatus
    } as UploadHandshakeConfirmation;

    var response = await APICall("user/UploadHandshake", "POST", confirmation);

    return response.success;
}

export async function SubmitQuizToServer(answers: TaskQuizAnswers) {
    let res = await APICall<undefined, TaskQuizAnswers>("Task", "POST", answers);

    return res;
}

export async function GetQuizQuestions(): Promise<APIResponse<TaskQuestion[]>> {
    let res = await APICall<TaskQuestion[], TaskQuizAnswers>("Quiz", "GET");

    return res;
} 

export async function GetDashboardPoints(): Promise<UserPointsJSON | null> {
    var res = await APICall<UserPointsJSON>("user/userpoints","GET", null);

    if (!res.success) {
        alert('Failed to get User Points.')
        return null;
    }

    return res.data!;
}

export async function SendUserConsent(consent: BizDocumentType, isGranted: boolean = false): Promise<APIResponse<ConsentResponse>> {
    let data = {
        ConsentType: consent,
        IsGranted: isGranted
    };

    let res = await APICall<ConsentResponse>("user/consent", "POST", data);

    return res;
}