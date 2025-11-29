import { UserTaskStatus, type ITask } from "../models/ViewModels";
import { UseTaskContext } from "../contexts/TaskViewContext";


function Task({ task }: { task: ITask }) {
    const taskWindowContext = UseTaskContext();
    const [viewedTask, setViewedTask] = [taskWindowContext.viewedTask, taskWindowContext.setViewedTask ]

    return (
        <div>
            <h3>{task.TaskTitle} </h3>
            <p>{ task.TaskDescription }</p>
            <p>Reward: { task.Reward }</p>
            <p>Status: { UserTaskStatus[task.Status] }</p>
            <button type="submit" className="submit-btn" onClick={() => { setViewedTask(task) }}>View Task</button>
        </div>
  );
}

export default Task;