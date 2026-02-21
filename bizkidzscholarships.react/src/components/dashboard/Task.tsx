import { UserTaskStatus, type ITask } from "../../models/ViewModels";
import { UseTaskContext } from "../../contexts/TaskViewContext";
import { useEffect, useState } from "react";


function Task({ task }: { task: ITask }) {
    const taskWindowContext = UseTaskContext();
    const [pointsClasses, setPointsClasses] = useState<string>("bg-dark bg-opacity-75");
    const [setViewedTask] = [taskWindowContext.setViewedTask ]

    const taskStatus = task.Status;

    useEffect(() => {
        switch (task.Status) {
            case UserTaskStatus.Completed:
                setPointsClasses("bg-success");
                break;
            case UserTaskStatus.Pending:
                setPointsClasses("bg-primary");
                break;
            case UserTaskStatus.Disabled:
                setPointsClasses("bg-dark bg-opacity-25");
                break;
            default:
                break;
        }
    }, []);

    return (
        <div className="card h-100">
            {task.TaskImageKey && (<>
                <div className="position-relative" style={{ height: "300px", overflow: "hidden" }}>

                    <img src={task.TaskImageKey} className="card-img-top w-100 h-100" style={{ objectFit: "cover", objectPosition: "0% 0%" }}></img>

                    <div className={`position-absolute top-0 end-0 m-2 px-3 py-1 m-4 rounded-pill text-white small shadow-lg fs-5 ${ pointsClasses }`}>
                        {task.Reward } points
                    </div>

                </div>
            </>)}
            
            <div className="card-body p-4">
                <h3 className="mb-0">{task.TaskTitle} </h3>

                <p>{ task.TaskDescription }</p>
                
                
            </div>

            <div className="card-body">
                {taskStatus == UserTaskStatus.Open && <button type="submit" className="btn btn-success" onClick={() => { setViewedTask(task) }}>View Task</button>}
                {taskStatus == UserTaskStatus.Disabled && <button type="submit" className="btn btn-warning" disabled>Unavailable</button>}
                {taskStatus == UserTaskStatus.Completed && <button type="submit" className="btn btn-success" disabled>Task Submitted</button>}
                {taskStatus == UserTaskStatus.Pending && <button type="submit" className="btn btn-dark">Under Review</button>}
            </div>

        </div>
  );
}

export default Task;