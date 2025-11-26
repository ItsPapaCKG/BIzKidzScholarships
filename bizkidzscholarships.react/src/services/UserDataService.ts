import { type PresignedURLData, type ITask } from "../models/ViewModels";
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
export async function TaskUploadChange(file: File | undefined): Promise<boolean> {
    if (file == undefined) {
        alert("Invalid upload.") 
        return false;
    }

    let { url, fields, key } = await GetPresignedS3Url(file.name.split(".").pop()!.toLowerCase());

    if (url == null || fields == null) {
        console.error("Could not get Presigned POST from AWS.");
        return false;
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
        alert("Upload successful! See: " + url + key)
        return true;
    }

    let responseText = await upload.text().catch(() => "(no body)");

    console.log(responseText);
    alert("Upload failed.");
    return false;
}

async function GetPresignedS3Url(ext: string) {
    var data = {
        "extension": ext
    }

    var res = await APICall<PresignedURLData>("user/GetPresignedURL", "POST", data);

    if (res.success) {
        return {url: res.data.url, fields: res.data.fields, key: res.data.key};
    }

    return { url: null, fields: null, key: null }
}

//async function UploadS3FileToURL(PostData: PresignedURLData, file: File) {
    
//}