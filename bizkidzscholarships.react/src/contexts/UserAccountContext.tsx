import { createContext, useContext, useState, type ReactNode } from "react";
import type { IUserProfile } from "../models/ViewModels";

export type userAccountContextType = {
    isAuthenticated: boolean,
    setIsAuthenticated: React.Dispatch<React.SetStateAction<boolean>>,
    userHasNoProfile: boolean,
    setUserHasNoProfile: React.Dispatch<React.SetStateAction<boolean>>,
    GetUserProfile: () => Promise<IUserProfile | null>

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

    const GetUserProfile = async () => {
        var res = await fetch("https://localhost:7095/api/user", {
            credentials: "include",
        });

        if (!res.ok) {
            console.log("[User Profile] Unspecified error!");
            var response = await res.text();
            console.log(`[User Profile] Text: ${response}`);

            if (res.status == 400) {
                setUserHasNoProfile(true);
            }
        }
        else {
            var rjson = await res.json();
            var jsonprofile: UserProfileJSON = rjson as UserProfileJSON;

            var profile: IUserProfile = {
                KidFullName: jsonprofile.firstName + " " + jsonprofile.lastName,
                BusinessEmail: jsonprofile.businessEmail,
                BusinessName: jsonprofile.businessName,
                BusinessPhone: jsonprofile.phoneNumber,
                BusinessLogoURL: jsonprofile.businessLogoKey
            }

            return profile;
        }

        return null;
    }

  return (
      <UserAccountContext.Provider value={{ isAuthenticated, setIsAuthenticated, userHasNoProfile, setUserHasNoProfile, GetUserProfile }}>
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