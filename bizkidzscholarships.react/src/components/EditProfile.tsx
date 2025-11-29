import { useEffect, useState, type ChangeEvent } from "react";
import { useNavigate } from "react-router-dom";
import { ActionType, type IUserProfile } from "../models/ViewModels";
import { UseUserAccountContext } from "../contexts/UserAccountContext";
import FileUpload from "./tasks/FileUpload";

function EditProfile() {
    const [errorState, setErrorState] = useState("")

    const navigate = useNavigate();
    const userAccountContext = UseUserAccountContext();
    const [userProfile, setUserProfile] = [userAccountContext.userProfile, userAccountContext.setUserProfile]
    const [editMode, setEditMode] = [userAccountContext.editMode, userAccountContext.setEditMode]
    const [profilePictureURL, setProfilePictureUrl] = useState<string>("");

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        // validate inputs
        e.preventDefault();

        var endpoint = userAccountContext.userHasNoProfile ? "registerprofile" : "updateprofile";
        var httpMethod = userAccountContext.userHasNoProfile ? "POST" : "PUT";

        // attempt registration
        var res = await fetch(`https://localhost:7095/api/user/${endpoint}`, {
            method: httpMethod,
            headers: { "Content-Type": "application/json" },
            credentials: "include",
            body: JSON.stringify(userProfile)
        });

        // catch errors
        if (!res.ok) {
            var txt = await res.text();
            console.log(`[Register Profile] Server Response: ${txt}`);
            setErrorState(txt);
            return
        }

        if (profilePictureURL != "") {
            userProfile.BusinessLogoKey = profilePictureURL;
        }

        //await setTimeout(() => { userAccountContext.setUserHasNoProfile(false); }, 5000);
        userAccountContext.setUserProfile(userProfile);
        userAccountContext.setUserHasNoProfile(false);
        setEditMode(false);
    };

    const handleChange = (e: ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;

        setUserProfile(prev => ({...prev, [name]: value }));
    }

    useEffect(() => {

    },[])
    // onChange={handleChange}
    return (
        <div className="border-2 border-danger">
            <form onSubmit={handleSubmit}>
                <label>Business Logo:
                    <FileUpload action={ActionType.ProfileImageUpload} setFileUrl={setProfilePictureUrl}/>
                </label>

                <label>First Name:
                    <input name="FirstName" value={userProfile.FirstName} onChange={handleChange} />
                </label>

                <label>Last Name:
                    <input name="LastName" value={userProfile.LastName} onChange={handleChange} />
                </label>

                <label>Business Name:
                    <input name="BusinessName" value={userProfile.BusinessName} onChange={handleChange} />
                </label>

                <label>Business Email:
                    <input name="BusinessEmail" value={userProfile.BusinessEmail} onChange={handleChange} />
                </label>

                <label>Business Phone:
                    <input name="PhoneNumber" value={userProfile.PhoneNumber} onChange={handleChange} />
                </label>

                <button type="submit">{ userAccountContext.userHasNoProfile ? "Submit Profile" : "Save Profile" }</button>

                <p className="danger">{errorState}</p>
            </form>
        </div>
    );
}

export default EditProfile;