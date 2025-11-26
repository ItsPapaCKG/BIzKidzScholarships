import { useRef } from "react";
import { UseViewedTaskContext } from "../../contexts/TaskViewContext";
import { TaskType } from "../../models/ViewModels";
import ImageUpload from "./ImageUpload";

function TaskWindow() {
    
    const viewedTask = UseViewedTaskContext();
    const [task, setTask] = [viewedTask.viewedTask, viewedTask.setViewedTask];

    return (
      <>
      { task !== null && (
      <div className="popup-background">
          <div className="popup-window">
              <button type="button" className="btn-close popup-close" aria-label="Close"></button>
              <div className="popup-body">
                  
                    <h1>{task.taskTitle}</h1>
                    <p>{task.taskDescription}</p>
                  {task.TaskType == TaskType.ImageUpload && ( <ImageUpload/> )}
                  
              </div>
          </div>
          </div>
          ) }
      </>
  );
}

export default TaskWindow;