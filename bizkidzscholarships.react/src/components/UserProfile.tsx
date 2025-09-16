import { useState } from "react";
import { useUserProfileContext } from "../context";
import "../models/ViewModels"
import type { IUserProfile } from "../models/ViewModels";

function UserProfile() {
    const [userProfile, setUserProfile] = useState<IUserProfile | null>(null);

    if (userProfile == null) return <p>Loading Profile...</p>
    return (
        <div className="d-flex gap-3" >
            <img src={ userProfile.BusinessLogoURL }/>

            <p className="p-2">
                <strong>Business Name</strong>: {userProfile.BusinessName}
            </p>

            <p className="p-2">
                <strong>Business Email</strong>: {userProfile.BusinessEmail}
            </p>

            <p className="p-2">
                <strong>Owner Name</strong>: {userProfile.KidFullName}
            </p>

            <p className="p-2">
                <strong>Phone</strong>: {userProfile.BusinessPhone}
            </p>
      </div>
  );
}

export default UserProfile;