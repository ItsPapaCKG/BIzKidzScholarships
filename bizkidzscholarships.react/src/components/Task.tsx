import type { ITask } from "../models/ViewModels";

interface taskProps {
    task: ITask
}

function Task({ task }: taskProps) {
    return (
        <div>
            <h3>{task.Title} </h3>
            <p>{ task.Description }</p>
            <p>{ task.Points }</p>
            <p>Status: { task.Status }</p>
        </div>
  );
}

export default Task;