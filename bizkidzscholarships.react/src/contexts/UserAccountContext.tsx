import { createContext, useContext, useState, type ReactNode } from "react";
import type { IUserProfile, UserCookieJSON } from "../models/ViewModels";

export type userAccountContextType = {
    isAuthenticated: boolean,
    setIsAuthenticated: React.Dispatch<React.SetStateAction<boolean>>,
    userHasNoProfile: boolean,
    setUserHasNoProfile: React.Dispatch<React.SetStateAction<boolean>>,
    GetUserProfile: () => Promise<IUserProfile | null>
    userProfile: IUserProfile,
    setUserProfile: React.Dispatch<React.SetStateAction<IUserProfile>>
    editMode: boolean,
    setEditMode: React.Dispatch<React.SetStateAction<boolean>>,
    userCookie: UserCookieJSON,
    setUserCookie: React.Dispatch<React.SetStateAction<UserCookieJSON>>
}
interface UserProfileJSON {
    userId: number,
    firstName: string,
    lastName: string,
    phoneNumber: string,
    businessEmail: string,
    businessName: string
    businessLogoKey: string
}

const UserAccountContext = createContext<userAccountContextType>({} as userAccountContextType);

function UserAccountProvider({ children }: { children: ReactNode }) {
    const [isAuthenticated, setIsAuthenticated] = useState(false);
    const [userHasNoProfile, setUserHasNoProfile] = useState(false);
    const [userProfile, setUserProfile] = useState({} as IUserProfile);
    const [editMode, setEditMode] = useState(false);
    const [userCookie, setUserCookie] = useState<UserCookieJSON>({} as UserCookieJSON)

    const GetUserProfile = async () => {
        var res = await fetch("https://localhost:7095/api/user", {
            credentials: "include",
        });

        if (res.status == 400) {
            setUserHasNoProfile(true);
        }

        if (!res.ok) {
            var response = await res.text();
            if (response == "") response = "Server did not provide an error.";

            console.log(`[User Profile] Text: ${response}`);
            return null;
        }

        var rjson = await res.json();
        var jsonprofile: UserProfileJSON = rjson as UserProfileJSON;

        var profile: IUserProfile = {
            FirstName: jsonprofile.firstName,
            LastName: jsonprofile.lastName,
            BusinessEmail: jsonprofile.businessEmail,
            BusinessName: jsonprofile.businessName,
            PhoneNumber: jsonprofile.phoneNumber,
            BusinessLogoKey: jsonprofile.businessLogoKey
        }

        setUserProfile(profile);
        return profile;
    }

  return (
      <UserAccountContext.Provider value={{ isAuthenticated, setIsAuthenticated, userHasNoProfile, setUserHasNoProfile, GetUserProfile, userProfile, setUserProfile, editMode, setEditMode, userCookie, setUserCookie }}>
          { children }
      </UserAccountContext.Provider>
  );
}

export function UseUserAccountContext(): userAccountContextType {
    var ctx = useContext(UserAccountContext);

    if (!ctx) throw new Error("UseUserAccountContext must be used inside of the UserAccountProvider.")

    return ctx;
}

export default UserAccountProvider;