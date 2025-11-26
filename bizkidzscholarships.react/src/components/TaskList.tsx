import { useEffect, useState } from "react";
import type { ITask } from "../models/ViewModels";
import Task from "./Task";
import { GetUserTasks } from "../services/UserDataService";
import ImageUpload from "./tasks/ImageUpload";

function TasksList() {
    const [tasks, setTasks] = useState<ITask[]>([])

    useEffect(() => {
        const getTasks = async () => {
            var userTasks = await GetUserTasks();

            setTasks(userTasks);
        }

        getTasks();
    }, []);

  return (
      <div>
          {
              tasks.map((task) => {
                  return <Task key={ task.taskId } task={ task } />
              }) 
          }

          <ImageUpload />
      </div>
  );
}

export default TasksList;