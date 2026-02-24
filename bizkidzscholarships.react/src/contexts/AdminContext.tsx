import { createContext, useContext, useState, type ReactNode } from "react";
import { type UserResult, type UserActivityLog, type AdminTaskSubmission } from "../models/ViewModels";

export type adminContextType = {
    userActivities: UserActivityLog[],
    setUserActivities: React.Dispatch<React.SetStateAction<UserActivityLog[]>>,
    userResults: UserResult[],
    setUserResults: React.Dispatch<React.SetStateAction<UserResult[]>>
}

const AdminContext = createContext<adminContextType>({} as adminContextType);

function AdminProvider({ children }: { children: ReactNode }) {
    const [userActivities, setUserActivities] = useState<UserActivityLog[]>([])
    const [userResults, setUserResults] = useState<UserResult[]>([])
    const [taskSubmissions, setTaskSubmissions] = useState<AdminTaskSubmission[]>([]);

    // TODO
    const getTaskSubmissions = async () => {

    };

    // TODO getActivities

    // TODO getUsers

  return (
      <AdminContext.Provider value={{ userActivities, setUserActivities, userResults, setUserResults }}>
          { children }
      </AdminContext.Provider>
  );
}

export function UseAdminContext(): adminContextType {
    var ctx = useContext(AdminContext);

    if (!ctx) throw new Error("AdminContext must be used inside of the AdminProvider.")

    return ctx;
}

export default AdminProvider;