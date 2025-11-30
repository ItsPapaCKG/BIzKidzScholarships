import { useEffect, useState } from "react";
import Task from "./Task";
import { GetUserTasks } from "../../services/UserDataService";
import { UseTaskContext } from "../../contexts/TaskViewContext";
import TaskWindow from "../tasks/TaskWindow";

function TasksList() {
    const taskContext = UseTaskContext();
    const [viewedTask, setViewedTask] = [taskContext.viewedTask, taskContext.setViewedTask ]
    const [tasks, setTasks] = [taskContext.tasks, taskContext.setTasks]
    const [taskRefresh, setTaskRefresh] = [taskContext.taskRefresh, taskContext.setTaskRefresh]

    const getTasks = async () => {
        var userTasks = await GetUserTasks();

        setTasks(userTasks);
    }

    useEffect(() => {
        getTasks();
    }, []);

    useEffect(() => {
        if (taskRefresh == true) {
            getTasks();
            setTaskRefresh(false);
        }
    }, [taskRefresh])

  return (
      <div>
        { viewedTask && (<p>Selected task: { viewedTask.TaskId }</p>) }
          {
              tasks.map((task) => {
                  return <Task key={ task.TaskId } task={ task } />
              }) 
          }
          <TaskWindow/>
      </div>
  );
}

export default TasksList;