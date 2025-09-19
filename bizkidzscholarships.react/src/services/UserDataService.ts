import type { ITask, IUserProfile } from "../models/ViewModels";

interface UserProfileJSON {
    userId: number,
    firstName: string,
    lastName: string,
    phoneNumber: string,
    businessEmail: string,
    businessName: string
    businessLogoKey: string
}

export default async function GetUserProfile(): Promise<IUserProfile | null> {
    var res = await fetch("https://localhost:7095/api/user", {
        credentials: "include",
    });

    if (!res.ok)
        console.log("[User Profile] Unspecified error!");

    var rjson = await res.json();
    var jsonprofile: UserProfileJSON = rjson as UserProfileJSON;

    var profile: IUserProfile = {
        KidFullName: jsonprofile.firstName + " " + jsonprofile.lastName,
        BusinessEmail: jsonprofile.businessEmail,
        BusinessName: jsonprofile.businessName,
        BusinessPhone: jsonprofile.phoneNumber,
        BusinessLogoURL: jsonprofile.businessLogoKey
    }

    return profile;
}

export async function GetUserTasks(): Promise<ITask[]> {
    var res = await fetch("https://localhost:7095/api/user/tasks", {
        credentials: "include"
    });

    var jsonResult = await res.json();

    var tasks = jsonResult as ITask[];

    return tasks;
}