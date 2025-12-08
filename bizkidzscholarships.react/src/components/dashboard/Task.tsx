import { UserTaskStatus, type ITask } from "../../models/ViewModels";
import { UseTaskContext } from "../../contexts/TaskViewContext";


function Task({ task }: { task: ITask }) {
    const taskWindowContext = UseTaskContext();
    const [setViewedTask] = [taskWindowContext.setViewedTask ]

    const taskStatus = task.Status;

    return (
        <div className="card">
            <div className="card-header">
                <h3 className="mb-0">{task.TaskTitle} </h3>
            </div>
            
            <div className="card-body">
                <p>{ task.TaskDescription }</p>
                <p>Reward: { task.Reward }</p>
                
                { taskStatus == UserTaskStatus.Open && <button type="submit" className="btn btn-success" onClick={() => { setViewedTask(task) }}>View Task</button>}
                { taskStatus == UserTaskStatus.Disabled && <button type="submit" className="btn btn-warning" disabled>Unavailable</button>}
                { taskStatus == UserTaskStatus.Completed && <button type="submit" className="btn btn-success" disabled>Task Submitted</button>}
                { taskStatus == UserTaskStatus.Pending && <button type="submit" className="btn btn-info">Under Review</button>}
            </div>
            
        </div>
  );
}

export default Task;