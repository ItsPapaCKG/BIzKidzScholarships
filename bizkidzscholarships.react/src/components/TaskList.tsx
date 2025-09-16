import { useState } from "react";
import { useTasksContext } from "../context";
import type { ITask } from "../models/ViewModels";
import Task from "./Task";

function TasksList() {
    const [tasks, setTasks] = useState<ITask[]>([])

  return (
      <div>
          {
              tasks.map((task, index, array) => {
                  return <Task task={ task } />
              }) 
          }
      </div>
  );
}

export default TasksList;