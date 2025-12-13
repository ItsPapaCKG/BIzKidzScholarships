import { createContext, useContext, useState, type ReactNode } from "react";
import type { ITask, IUserPoints, TaskList } from "../models/ViewModels";
import { GetDashboardPoints } from "../services/UserDataService";


export type viewedContextType = {
    viewedTask: ITask | null,
    setViewedTask: React.Dispatch<React.SetStateAction<ITask | null>>,
    handshakeRequestId: string | null
    setHandshakeRequestId: React.Dispatch<React.SetStateAction<string | null>>,
    RequestPoints: () => Promise<IUserPoints>,
    tasks: TaskList,
    setTasks: React.Dispatch<React.SetStateAction<TaskList>>,
    taskRefresh: boolean,
    setTaskRefresh: React.Dispatch<React.SetStateAction<boolean>>,
    points: IUserPoints,
    setPoints: React.Dispatch<React.SetStateAction<IUserPoints>>,
    setPointsRefresh: React.Dispatch<React.SetStateAction<boolean>>,
    DoPointsRefresh: boolean
}

const TaskContext = createContext<viewedContextType>({} as viewedContextType);

function TaskProvider({ children }: { children: ReactNode }) {
    const [tasks, setTasks] = useState<TaskList>({Tasks: [], Loaded: false} as TaskList)
    const [viewedTask, setViewedTask] = useState<ITask | null>(null);
    const [handshakeRequestId, setHandshakeRequestId] = useState<string | null>(null);
    const [taskRefresh, setTaskRefresh] = useState<boolean>(false);
    const [DoPointsRefresh, setPointsRefresh] = useState<boolean>(false);
    const [points, setPoints] = useState<IUserPoints>({
            TotalPoints: 0,
            Entries: 0,
            Loaded: false
    } as IUserPoints);

    const RequestPoints = async (): Promise<IUserPoints> => {
        var res = await GetDashboardPoints();

        if (res == null) {
            return {
                TotalPoints: 0,
                Entries: 0,
                IsError: true,
                Loaded: true
            } as IUserPoints;
        }

        return {
            TotalPoints: res.points,
            Entries: res.entries,
            IsError: false,
            Loaded: true
        } as IUserPoints;
    }

  return (
      <TaskContext.Provider value={{ viewedTask, setViewedTask, handshakeRequestId, setHandshakeRequestId, RequestPoints, tasks, setTasks, taskRefresh, setTaskRefresh, points, setPoints, setPointsRefresh, DoPointsRefresh }}>
          { children }
      </TaskContext.Provider>
  );
}

export function UseTaskContext(): viewedContextType {
    var ctx = useContext(TaskContext);

    if (!ctx) throw new Error("UseViewedTaskContext must be used inside of the UserAccountProvider.")

    return ctx;
}

export default TaskProvider;