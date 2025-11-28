import { type PresignedURLData, type ITask, type ITaskJSON, type StartUploadRequest, type StartUploadHandshakeResponse, ActionType, type UploadHandshakeConfirmation, RequestStatus } from "../models/ViewModels";
import { APICall, type APIResponse } from "./APIService";

export async function CheckUserProfile(): Promise<boolean> {
    return false;
}

export async function GetUserTasks(): Promise<ITask[]> {
    var res = await fetch("https://localhost:7095/api/user/tasks", {
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
            TaskType: i.taskType
        } as ITask)
    });

    return tasks;
}

export async function TaskUploadChange(taskid: Number, file: File | undefined): Promise<boolean> {
    if (file == undefined) {
        alert("Invalid upload.") 
        return false;
    }
//file.name.split(".").pop()!.toLowerCase()

    let request = {
        ActionType: ActionType.TaskUpload,
        Extension: file.name.split(".").pop()!.toLowerCase(),
        TaskId: taskid
    } as StartUploadRequest

    let presignedDataResponse: StartUploadHandshakeResponse | null = await GetPresignedS3Url(request);

    if (presignedDataResponse == null || presignedDataResponse.PresignedData == null) {
        console.error("An internal error has occurred: No Presigned URL could be read from the server.");
        return false;
    }

    let presignedData = presignedDataResponse.PresignedData;

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
        return await CompleteUploadHandshake(presignedDataResponse.RequestId, RequestStatus.Success);
    }

    let responseText = await upload.text().catch(() => "(no body)");

    console.log(responseText);
    alert("Upload failed.");

    return await CompleteUploadHandshake(presignedDataResponse.RequestId, RequestStatus.Failed);
}

async function GetPresignedS3Url(request: StartUploadRequest)  {

    var res = await APICall<StartUploadHandshakeResponse>("user/NewUploadRequest", "POST", request);

    if (res.success) {
        return res.data;
    }

    return null;
}

async function CompleteUploadHandshake(requestId: string, requestStatus: RequestStatus) {
    var confirmation = {
        RequestId: requestId,
        RequestStatus: requestStatus
    } as UploadHandshakeConfirmation;

    var response = await APICall("user/UploadConfirmation", "POST", confirmation);

    return response.success;
}

//async function UploadS3FileToURL(PostData: PresignedURLData, file: File) {
    
//}