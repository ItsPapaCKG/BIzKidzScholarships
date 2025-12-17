import { createContext, useContext, useEffect, useState, type ReactNode } from "react";
import type { IUserProfile, UserCookieJSON } from "../models/ViewModels";
import { useNavigate } from "react-router-dom";

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
    populateCookie: () => Promise<void>,
    loadingData: boolean,
    setLoadingData: React.Dispatch<React.SetStateAction<boolean>>
}
interface UserProfileJSON {
    userId: number,
    firstName: string,
    lastName: string,
    phoneNumber: string,
    email: string,
    businessName: string
    businessLogoKey: string,
    birthday: Date
}

const UserAccountContext = createContext<userAccountContextType>({} as userAccountContextType);

function UserAccountProvider({ children }: { children: ReactNode }) {
    const [isAuthenticated, setIsAuthenticated] = useState(false);
    const [userHasNoProfile, setUserHasNoProfile] = useState(false);
    const [userProfile, setUserProfile] = useState({Birthday: new Date(), Loaded: false} as IUserProfile);
    const [editMode, setEditMode] = useState(false);
    const [userCookie, setUserCookie] = useState<UserCookieJSON>({roles: [] as string[]} as UserCookieJSON)
    const [loadingData, setLoadingData] = useState(true);

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
            Email: jsonprofile.email,
            BusinessName: jsonprofile.businessName,
            PhoneNumber: jsonprofile.phoneNumber,
            BusinessLogoKey: jsonprofile.businessLogoKey,
            Birthday: new Date(jsonprofile.birthday),
            Loaded: true
        }

        setUserProfile(profile);
        return profile;
    }

    const populateCookie = async () => {
            var res = await fetch("https://localhost:7095/auth/me", { credentials: "include" });
    
            if (!res.ok) {
                setIsAuthenticated(false);
                return;
            }
    
            var cookie = await res.json() as UserCookieJSON;
    
            setUserCookie(cookie);
            setIsAuthenticated(true);
            setLoadingData(false);
        }

    useEffect(()=>{
        populateCookie();
    }, []);

  return (
      <UserAccountContext.Provider value={{ isAuthenticated, setIsAuthenticated, userHasNoProfile, setUserHasNoProfile, GetUserProfile, userProfile, setUserProfile, editMode, setEditMode, userCookie, setUserCookie, populateCookie, loadingData, setLoadingData }}>
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
