import { createContext, useContext, useState, type ReactNode } from "react";
import type { UserActivityLog } from "../models/ViewModels";

export type adminContextType = {
    userActivities: UserActivityLog[],
    setUserActivities: React.Dispatch<React.SetStateAction<UserActivityLog[]>>
}

const AdminContext = createContext<adminContextType>({} as adminContextType);

function AdminProvider({ children }: { children: ReactNode }) {
    const [userActivities, setUserActivities] = useState<UserActivityLog[]>([])

  return (
      <AdminContext.Provider value={{ userActivities, setUserActivities }}>
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