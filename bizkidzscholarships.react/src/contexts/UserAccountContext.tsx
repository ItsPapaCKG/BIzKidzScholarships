import { createContext, useContext, useEffect, useState, type ReactNode } from "react";
import type { IUserProfile, UserCookieJSON, UserType } from "../models/ViewModels";
import { useNavigate } from "react-router-dom";
import { APICall } from "../services/APIService";

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
    setLoadingData: React.Dispatch<React.SetStateAction<boolean>>,
    logout: () => void,
    isAdmin: boolean,
    setIsAdmin: React.Dispatch<React.SetStateAction<boolean>>
}
interface UserProfileJSON {
    userId: number,
    firstName: string,
    lastName: string,
    phoneNumber: string,
    email: string,
    businessName: string
    businessLogoKey: string,
    birthday: Date,
    userType: UserType
}

const UserAccountContext = createContext<userAccountContextType>({} as userAccountContextType);

function UserAccountProvider({ children }: { children: ReactNode }) {
    const [isAuthenticated, setIsAuthenticated] = useState(false);
    const [userHasNoProfile, setUserHasNoProfile] = useState(false);
    const [userProfile, setUserProfile] = useState({Birthday: new Date(), Loaded: false} as IUserProfile);
    const [editMode, setEditMode] = useState(false);
    const [userCookie, setUserCookie] = useState<UserCookieJSON>({roles: [] as string[]} as UserCookieJSON)
    const [loadingData, setLoadingData] = useState(true);
    const [isAdmin, setIsAdmin] = useState(false);

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
            UserType: jsonprofile.userType,
            Loaded: true
        }

        setUserProfile(profile);
        return profile;
    }

    const populateCookie = async () => {
            var res = await fetch("https://localhost:7095/auth/me", { credentials: "include" });
    
            if (!res.ok) {
                setIsAuthenticated(false);
                setLoadingData(false);
                return;
            }
    
            var cookie = await res.json() as UserCookieJSON;
    
            setUserCookie(cookie);
            setIsAuthenticated(true);
            setLoadingData(false);
        }

    const logout = async () => {
        let res = await APICall("logout", "POST", null, true);

        if (res.success) {
            window.location.reload();
            return;
        }
    }


    useEffect(()=>{
        populateCookie();
    }, []);

    useEffect(() => {
        if (userCookie.roles.length == 0) {
            return;
        }

        if (userCookie.roles.includes("Admin")) {
            setIsAdmin(true);
            return;
        }

        setIsAdmin(false);
    }, [userCookie]);

  return (
      <UserAccountContext.Provider value={{ isAuthenticated, setIsAuthenticated, userHasNoProfile, setUserHasNoProfile, GetUserProfile, userProfile, setUserProfile, editMode, setEditMode, userCookie, setUserCookie, populateCookie, loadingData, setLoadingData, logout, isAdmin, setIsAdmin }}>
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
