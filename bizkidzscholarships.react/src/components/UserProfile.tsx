import { useContext, useEffect, useState } from "react";
import "../models/ViewModels"
import type { IUserProfile } from "../models/ViewModels";
import { UseUserAccountContext } from "../contexts/UserAccountContext";
import { useNavigate } from "react-router-dom";
import EditProfile from "./EditProfile";

function UserProfile() {
    const navigate = useNavigate();

    const userAccountContext = UseUserAccountContext();
    const userHasNotRegisteredProfile = userAccountContext.userHasNoProfile;

    const [userProfile, setUserProfile] = [userAccountContext.userProfile, userAccountContext.setUserProfile];
    const [editMode, setEditMode] = [userAccountContext.editMode, userAccountContext.setEditMode];

    useEffect(() => {
        var getProfile = async () => {
            var p = await userAccountContext.GetUserProfile();

            if (p == null)
                return;

            setUserProfile(p);
        }

        getProfile();
    }, [userHasNotRegisteredProfile]);

    if (userProfile == null && !userHasNotRegisteredProfile) return <p>Loading Profile...</p>
    else if (userHasNotRegisteredProfile && userProfile == null) return <EditProfile />
    else if (userProfile != null)
        return (
            <>
                { editMode ? 
                    <EditProfile />
                :
                    <div className="d-flex gap-3" >
                        <img src={userProfile.BusinessLogoKey} />

                        <p className="p-2">
                            <strong>Business Name</strong>: {userProfile.BusinessName}
                        </p>

                        <p className="p-2">
                            <strong>Business Email</strong>: {userProfile.BusinessEmail}
                        </p>

                        <p className="p-2">
                            <strong>Owner Name</strong>: {userProfile.FirstName + " " + userProfile.LastName}
                        </p>

                        <p className="p-2">
                            <strong>Phone</strong>: {userProfile.PhoneNumber}
                        </p>

                        <button onClick={e => setEditMode(true)}>Edit Profile</button>
                    </div>
                }
            </>
      );
}

export default UserProfile;