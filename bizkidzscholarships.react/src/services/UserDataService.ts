import type { IUserProfile } from "../models/ViewModels";

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
        method: "GET",
        headers: { "Content-Type": "application/json" },
        credentials: "include",
    });

    if (!res.ok)
        throw new Error("Unspecified error");

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