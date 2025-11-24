import { object } from "zod";
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
export async function TaskUploadChange(file: File | undefined) {
    if (file == undefined) {
        return alert("Invalid upload.");
    }

    let { url, fields } = await GetPresignedS3Url();

    if (url == null || fields == null) {
        console.error("Could not get Presigned POST from AWS.");
        return;
    }

    var formdata = new FormData();
    Object.entries(fields).forEach(([k, v]) => {
        formdata.append(k, v);
    });

    formdata.append("file",file);

    var upload = await fetch(url, {
        method: "POST",
        body: formdata
    })

    if (upload.ok) {
        return alert("Upload successful! See: " + url)
    }

    return alert("Upload failed.")
}

async function GetPresignedS3Url() {
    var res = await APICall<PresignedURLData>("user/GetPresignedURL", "GET", null);

    if (res.success) {
        return {url: res.data.Url, fields: res.data.Fields};
    }

    return { url: null, fields: null }
}

//async function UploadS3FileToURL(PostData: PresignedURLData, file: File) {
    
//}