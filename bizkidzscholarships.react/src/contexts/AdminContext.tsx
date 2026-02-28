import { createContext, useContext, useEffect, useState, type ReactNode } from "react";
import { type UserResult, type UserActivityLog, type AdminTaskSubmission, type ITask, type SubmissionsSearchResults, type GetTasksResponse } from "../models/ViewModels";
import { GetAllSubmissions, GetAllTasks, GetSubmissions } from "../services/AdminDataService";

export type adminContextType = {
    userActivities: UserActivityLog[],
    setUserActivities: React.Dispatch<React.SetStateAction<UserActivityLog[]>>,
    userResults: UserResult[],
    setUserResults: React.Dispatch<React.SetStateAction<UserResult[]>>,
    taskSubmissions: SubmissionsSearchResults | undefined,
    setTaskSubmissions: React.Dispatch<React.SetStateAction<SubmissionsSearchResults | undefined>>
    tasks: GetTasksResponse | undefined,
    setTasks: React.Dispatch<React.SetStateAction<GetTasksResponse | undefined>>,
    getTaskSubmissions: (taskId: number) => Promise<void>,
    getTasks: () => Promise<void>
}

const AdminContext = createContext<adminContextType>({} as adminContextType);

function AdminProvider({ children }: { children: ReactNode }) {
    const [userActivities, setUserActivities] = useState<UserActivityLog[]>([])
    const [userResults, setUserResults] = useState<UserResult[]>([])
    const [taskSubmissions, setTaskSubmissions] = useState<SubmissionsSearchResults | undefined>();
    const [tasks, setTasks] = useState<GetTasksResponse | undefined>()

    // TODO
    const getTaskSubmissions = async () => {
        let submissions = await GetAllSubmissions();

        setTaskSubmissions(submissions);
    };

    const getTasks = async () => {
        let tasks = await GetAllTasks();
        
        setTasks(tasks);
    }

    // TODO getActivities

    // TODO getUsers
    const getUsers = async () => {

    }

    useEffect(() => {
        getTaskSubmissions();
    }, []);

  return (
      <AdminContext.Provider value={{ userActivities, setUserActivities, userResults, setUserResults, taskSubmissions, setTaskSubmissions, tasks, setTasks, getTaskSubmissions, getTasks }}>
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