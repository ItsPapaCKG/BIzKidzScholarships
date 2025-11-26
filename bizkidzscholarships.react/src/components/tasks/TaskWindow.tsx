import { UseViewedTaskContext } from "../../contexts/TaskViewContext";
import { TaskType } from "../../models/ViewModels";
import ImageUpload from "./ImageUpload";
import Quiz from "./Quiz";
import SocialMedia from "./SocialMedia";
import VideoUpload from "./VideoUpload";

function TaskWindow() {

    const viewedTask = UseViewedTaskContext();
    const [task, setTask] = [viewedTask.viewedTask, viewedTask.setViewedTask];

    return (
      <>
      { task !== null && (
      <div className="popup-background">
          <div className="popup-window">
              <button type="button" className="btn-close popup-close" aria-label="Close" onClick={ () => { setTask(null) } }></button>
              <div className="popup-body">
                  
                    <h1>{task.taskTitle}</h1>
                    <p>{task.taskDescription}</p>
                    {/* <p>{task.taskType}</p> */}

                    {task.taskType == TaskType.SocialMedia && <SocialMedia/> }
                    {task.taskType == TaskType.ImageUpload && <ImageUpload/> }
                    {task.taskType == TaskType.VideoUpload && <VideoUpload/> }
                    {task.taskType == TaskType.Quiz && <Quiz/> }
                    {task.taskType == TaskType.Contest && <p>Contest goes here</p> }
                  
              </div>
          </div>
          </div>
          ) }
      </>
  );
}

export default TaskWindow;