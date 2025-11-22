import { UseUserAccountContext } from "../contexts/UserAccountContext";
import { type PresignedURLData, type ITask, type IUserProfile } from "../models/ViewModels";
import { APICall } from "./APIService";

export async function CheckUserProfile(): Promise<boolean> {
    return false;
}

export async function GetUserTasks(): Promise<ITask[]> {
    var res = await fetch("https://localhost:7095/api/user/tasks", {
        credentials: "include"
    });

    var jsonResult = await res.json();

    var tasks = jsonResult as ITask[];

    return tasks;
}

// input uploads a file
// onchange: getpresigned URL
// POST file with presigned fields
// get response. If successful
// API Call to set URL of object attached to field

async function GetPresignedS3Url() {
    var res = await APICall<PresignedURLData>("/GetPresignedURL", "GET", null);

    if (res.success) {
        return res;
    }
}

async function UploadS3FileToURL(PostData: PresignedURLData, file: File) {
    var res = await fetch
}