import { UserTaskStatus, type ITask } from "../../models/ViewModels";
import { UseTaskContext } from "../../contexts/TaskViewContext";


function Task({ task }: { task: ITask }) {
    const taskWindowContext = UseTaskContext();
    const [setViewedTask] = [taskWindowContext.setViewedTask ]

    return (
        <div className="card">
            <div className="card-header">
                <h3 className="mb-0">{task.TaskTitle} </h3>
            </div>
            
            <div className="card-body">
                <p>{ task.TaskDescription }</p>
                <p>Reward: { task.Reward }</p>
                <p>Status: { UserTaskStatus[task.Status] }</p>
                <button type="submit" className="btn btn-success" onClick={() => { setViewedTask(task) }}>View Task</button>
            </div>
            
        </div>
  );
}

export default Task;