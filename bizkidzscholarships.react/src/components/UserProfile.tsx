import { useUserProfileContext } from "../context";
import "../models/ViewModels"

function UserProfile() {
    const profile = useUserProfileContext();

    return (
        <div className="d-flex gap-3" >
            <img src={ profile.BusinessLogoURL }/>

            <p className="p-2">
                <strong>Business Name</strong>: {profile.BusinessName}
            </p>

            <p className="p-2">
                <strong>Business Email</strong>: {profile.BusinessEmail}
            </p>

            <p className="p-2">
                <strong>Owner Name</strong>: {profile.KidFullName}
            </p>

            <p className="p-2">
                <strong>Phone</strong>: {profile.BusinessPhone}
            </p>
      </div>
  );
}

export default UserProfile;