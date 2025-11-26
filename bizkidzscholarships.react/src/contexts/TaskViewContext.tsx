import { createContext, useContext, useState, type ReactNode } from "react";
import type { ITask } from "../models/ViewModels";


export type viewedContextType = {
    viewedTask: ITask | null,
    setViewedTask: React.Dispatch<React.SetStateAction<ITask | null>>,
}

const ViewedTaskContext = createContext<viewedContextType>({} as viewedContextType);

function ViewedTaskProvider({ children }: { children: ReactNode }) {
    const [viewedTask, setViewedTask] = useState<ITask | null>(null);

  return (
      <ViewedTaskContext.Provider value={{ viewedTask, setViewedTask }}>
          { children }
      </ViewedTaskContext.Provider>
  );
}

export function UseViewedTaskContext(): viewedContextType {
    var ctx = useContext(ViewedTaskContext);

    if (!ctx) throw new Error("UseViewedTaskContext must be used inside of the UserAccountProvider.")

    return ctx;
}

export default ViewedTaskProvider;