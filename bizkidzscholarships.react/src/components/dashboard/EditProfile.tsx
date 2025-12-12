import { useEffect, useState, type ChangeEvent } from "react";
import { useNavigate } from "react-router-dom";
import { ActionType, type IUserProfile } from "../../models/ViewModels";
import { UseUserAccountContext } from "../../contexts/UserAccountContext";
import FileUpload from "../tasks/FileUpload";

function EditProfile() {
    const [errorState, setErrorState] = useState("")

    const navigate = useNavigate();
    const userAccountContext = UseUserAccountContext();
    const [userProfile, setUserProfile] = [userAccountContext.userProfile, userAccountContext.setUserProfile]
    const [editMode, setEditMode] = [userAccountContext.editMode, userAccountContext.setEditMode]
    const [profilePictureURL, setProfilePictureUrl] = useState<string>("");
    const [hasDataChanged, dataChanged] = useState<boolean>(false);

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        // validate inputs
        e.preventDefault();

        if (!hasDataChanged) {
            setEditMode(false);
            return;
        }

        var endpoint = userAccountContext.userHasNoProfile ? "registerprofile" : "updateprofile";
        var httpMethod = userAccountContext.userHasNoProfile ? "POST" : "PUT";

        if (profilePictureURL != "") {
            userProfile.BusinessLogoKey = profilePictureURL;
        }

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

        //await setTimeout(() => { userAccountContext.setUserHasNoProfile(false); }, 5000);
        userAccountContext.setUserProfile(userProfile);
        userAccountContext.setUserHasNoProfile(false);
        dataChanged(false);
        setEditMode(false);
    };

    const handleChange = (e: ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;

        dataChanged(true);

        setUserProfile(prev => ({...prev, [name]: value }));
    }

    const changeProfile = (profileUrl: string) => {
        setProfilePictureUrl(profileUrl);
        dataChanged(true);
    };

    // onChange={handleChange}
    return (
        <>
        <div className="card-body container p-4">
            <form onSubmit={handleSubmit}>
                <div className="row justify-content-center m-4">
                    <div className="col-12 col-md-8">
                        <img src={userProfile.BusinessLogoKey} className="m-0"/>
                    </div>
                </div>

            
                <div className="row mb-3">
                    <div className="col">
                        
                        <div className="input-group mb-3">
                            <span className="input-group-text" id="inputGroup-sizing-default">Logo</span>
                            <FileUpload action={ActionType.ProfileImageUpload} setFileUrl={changeProfile}/>
                            {/* <span className="input-group-text">Logo saved</span> */}
                        </div>
                        <div className="input-group mb-3">
                            <span className="input-group-text" id="inputGroup-sizing-default">Business Name</span>
                            <input type="text" name="BusinessName" className="form-control" aria-label="Sizing example input" aria-describedby="inputGroup-sizing-default" onChange={handleChange} value={userProfile.BusinessName}/>
                        </div>

                        <div className="input-group mb-3">
                            <span className="input-group-text" id="inputGroup-sizing-default">Email</span>
                            <input type="text" name="BusinessEmail" className="form-control" aria-label="Sizing example input" aria-describedby="inputGroup-sizing-default" onChange={handleChange}  value={userProfile.BusinessEmail}/>
                        </div>

                        <div className="input-group mb-3">
                            <span className="input-group-text" id="inputGroup-sizing-default">Phone Number</span>
                            <input type="text" name="PhoneNumber" className="form-control" aria-label="Sizing example input" aria-describedby="inputGroup-sizing-default" onChange={handleChange} value={userProfile.PhoneNumber}/>
                        </div>
                        
                    </div>    
                </div>
        
                <div className="row">
                    <div className="col d-grid">
                        <button type="submit" className="btn btn-lg btn-success">{ userAccountContext.userHasNoProfile ? "Submit Profile" : hasDataChanged ? "Save Profile" : "Cancel" }</button>
                    </div>
                </div>
            </form>
        </div>
        </>
    );
}

export default EditProfile;