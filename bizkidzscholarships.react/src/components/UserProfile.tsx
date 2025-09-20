import { useContext, useEffect, useState } from "react";
import "../models/ViewModels"
import type { IUserProfile } from "../models/ViewModels";
import { UseUserAccountContext } from "../contexts/UserAccountContext";
import { useNavigate } from "react-router-dom";

function UserProfile() {
    const [userProfile, setUserProfile] = useState<IUserProfile | null>(null);
    const navigate = useNavigate();

    const userAccountContext = UseUserAccountContext();
    const userHasNotRegisteredProfile = userAccountContext.userHasNoProfile;

    useEffect(() => {
        var getProfile = async () => {
            var p = await userAccountContext.GetUserProfile();

            if (p == null)
                return;

            setUserProfile(p);
        }

        getProfile();
    }, []);

    if (userProfile == null && !userHasNotRegisteredProfile) return <p>Loading Profile...</p>
    else if (userHasNotRegisteredProfile && userProfile == null) return <p>You have not registered a profile!</p>
    else if (userProfile != null)
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