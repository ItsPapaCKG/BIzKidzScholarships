import { UserTaskStatus, type ITask } from "../models/ViewModels";


function Task({ task }: { task: ITask }) {
    return (
        <div>
            <h3>{task.taskTitle} </h3>
            <p>{ task.taskDescription }</p>
            <p>{ task.reward }</p>
            <p>Status: { UserTaskStatus[task.status] }</p>
        </div>
  );
}

export default Task;