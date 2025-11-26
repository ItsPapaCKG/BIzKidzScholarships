import { useEffect, useState } from "react";
import type { ITask } from "../models/ViewModels";
import Task from "./Task";
import { GetUserTasks } from "../services/UserDataService";
import ImageUpload from "./tasks/TaskWindow";
import { UseViewedTaskContext } from "../contexts/TaskViewContext";
import TaskWindow from "./tasks/TaskWindow";

function TasksList() {
    const [tasks, setTasks] = useState<ITask[]>([])

    const taskWindowContext = UseViewedTaskContext();
    const [viewedTask, setViewedTask] = [taskWindowContext.viewedTask, taskWindowContext.setViewedTask ]

    useEffect(() => {
        const getTasks = async () => {
            var userTasks = await GetUserTasks();

            setTasks(userTasks);
        }

        getTasks();
    }, []);

  return (
      <div>
        { viewedTask && (<p>Selected task: { viewedTask.taskId }</p>) }
          {
              tasks.map((task) => {
                  return <Task key={ task.taskId } task={ task } />
              }) 
          }
          <TaskWindow/>
      </div>
  );
}

export default TasksList;