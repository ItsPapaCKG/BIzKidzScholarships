import { type PresignedURLData, type ITask, type ITaskJSON, type StartUploadRequest, type StartUploadHandshakeResponse, ActionType } from "../models/ViewModels";
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

// input uploads a file
// onchange: getpresigned URL
// POST file with presigned fields
// get response. If successful
// API Call to set URL of object attached to field
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

    let presignedData: PresignedURLData | null = await GetPresignedS3Url(request);

    if (presignedData == null) {
        console.error("Could not get Presigned POST from AWS.");
        return false;
    }

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
        alert("Upload successful! See: " + presignedData.url + presignedData.key)
        return true;
    }

    let responseText = await upload.text().catch(() => "(no body)");

    console.log(responseText);
    alert("Upload failed.");
    return false;
}

async function GetPresignedS3Url(request: StartUploadRequest)  {

    var res = await APICall<StartUploadHandshakeResponse>("user/NewUploadRequest", "POST", request);

    if (res.success) {
        return res.data.PresignedData;
    }

    return null;
}

//async function UploadS3FileToURL(PostData: PresignedURLData, file: File) {
    
//}