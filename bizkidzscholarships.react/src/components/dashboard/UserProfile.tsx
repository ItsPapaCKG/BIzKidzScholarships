import { useEffect } from "react";
import "../../models/ViewModels"
import { UseUserAccountContext } from "../../contexts/UserAccountContext";
//import { useNavigate } from "react-router-dom";
import EditProfile from "./EditProfile";

function UserProfile() {
    //const navigate = useNavigate();

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

    if (userHasNotRegisteredProfile) return <EditProfile />
    else if (!userProfile.Loaded) return (<></>)
    else if (userProfile != null)
        return (
            <>
                
                    <div className="d-flex gap-3 userProfile card flex-fill" >
                        { editMode ? 
                            <EditProfile />
                                :
                                <>
                        {/* <div className="card-header text-center">
                            <h4 className="mb-0">Your Business Profile</h4>
                        </div> */}

                        <div className="card-body p-2 m-3">
                            <div className="row mb-3">
                                <div className="col profile-ctn m-5">
                                    <img src={userProfile.BusinessLogoKey} />
                                </div>

                                <div className="col">
                                    <p className="p-2">
                                        <strong>Student Name</strong>: {userProfile.FirstName + " " + userProfile.LastName}
                                    </p>

                                    <p className="p-2">
                                        <strong>Birthday</strong>: {userProfile.Birthday.toDateString()}
                                    </p>
                                    
                                    <p className="p-2">
                                        <strong>Business Name</strong>: {userProfile.BusinessName}
                                    </p>

                                    <p className="p-2">
                                        <strong>Student Email</strong>: {userProfile.Email}
                                    </p>

                                    <p className="p-2">
                                        <strong>Phone</strong>: {userProfile.PhoneNumber}
                                    </p>
                                </div>    
                            </div>
                        
                            <div className="row">
                                <div className="col d-grid">
                                    <button onClick={() => setEditMode(true)} className="btn btn-primary">Edit Profile</button>
                                </div>
                            </div>
                            
                        </div>
                        </>
                        }
                    </div>
                
            </>
      );
}

export default UserProfile;