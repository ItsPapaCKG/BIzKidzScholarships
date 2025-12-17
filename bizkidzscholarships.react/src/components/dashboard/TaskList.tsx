import { useEffect, useState } from "react";
import Task from "./Task";
import { GetUserTasks } from "../../services/UserDataService";
import { UseTaskContext } from "../../contexts/TaskViewContext";
import TaskWindow from "../tasks/TaskWindow";
import type { TaskList } from "../../models/ViewModels";

function TasksList() {
    const taskContext = UseTaskContext();
    const [viewedTask, setViewedTask] = [taskContext.viewedTask, taskContext.setViewedTask ]
    const [taskList, setTasks] = [taskContext.tasks, taskContext.setTasks]
    const [taskRefresh, setTaskRefresh] = [taskContext.taskRefresh, taskContext.setTaskRefresh]

    const getTasks = async () => {
        var userTasks = await GetUserTasks();

        let taskList = { Tasks: userTasks, Loaded: true} as TaskList;

        setTasks(taskList);
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

  if (!taskList.Loaded) return (<></>)
  else return (
      <div className="row">
          {
              taskList.Tasks.map((task) => {
                  return (<div key={ task.TaskId } className="col-md-6 col-xs mb-3"><Task task={ task } /></div>);
              }) 
          }
          <TaskWindow/>
      </div>
  );
}

export default TasksList;