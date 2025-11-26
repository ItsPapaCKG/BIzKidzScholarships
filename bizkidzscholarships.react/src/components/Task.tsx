import { UserTaskStatus, type ITask } from "../models/ViewModels";
import { UseViewedTaskContext } from "../contexts/TaskViewContext";


function Task({ task }: { task: ITask }) {
    const taskWindowContext = UseViewedTaskContext();
    const [viewedTask, setViewedTask] = [taskWindowContext.viewedTask, taskWindowContext.setViewedTask ]

    return (
        <div>
            <h3>{task.taskTitle} </h3>
            <p>{ task.taskDescription }</p>
            <p>{ task.reward }</p>
            <p>Status: { UserTaskStatus[task.status] }</p>
            <button type="submit" className="submit-btn" onClick={() => { setViewedTask(task) }}>View Task</button>
        </div>
  );
}

export default Task;