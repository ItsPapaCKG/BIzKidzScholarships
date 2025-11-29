import { createContext, useContext, useState, type ReactNode } from "react";
import type { ITask } from "../models/ViewModels";


export type viewedContextType = {
    viewedTask: ITask | null,
    setViewedTask: React.Dispatch<React.SetStateAction<ITask | null>>,
    handshakeRequestId: string | null
    setHandshakeRequestId: React.Dispatch<React.SetStateAction<string | null>>,
    tasks: ITask[],
    setTasks: React.Dispatch<React.SetStateAction<ITask[]>>,
    taskRefresh: boolean,
    setTaskRefresh: React.Dispatch<React.SetStateAction<boolean>>
}

const TaskContext = createContext<viewedContextType>({} as viewedContextType);

function TaskProvider({ children }: { children: ReactNode }) {
    const [tasks, setTasks] = useState<ITask[]>([])
    const [viewedTask, setViewedTask] = useState<ITask | null>(null);
    const [handshakeRequestId, setHandshakeRequestId] = useState<string | null>(null);
    const [taskRefresh, setTaskRefresh] = useState<boolean>(false);

  return (
      <TaskContext.Provider value={{ viewedTask, setViewedTask, handshakeRequestId, setHandshakeRequestId, tasks, setTasks, taskRefresh, setTaskRefresh }}>
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