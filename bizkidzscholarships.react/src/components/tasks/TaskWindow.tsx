import { UseTaskContext } from "../../contexts/TaskViewContext";
import { TaskType } from "../../models/ViewModels";
import ImageUpload from "./ImageUpload";
import Quiz from "./Quiz";
import SocialMedia from "./SocialMedia";
import VideoUpload from "./VideoUpload";

function TaskWindow() {

    const viewedTask = UseTaskContext();
    const [task, setTask] = [viewedTask.viewedTask, viewedTask.setViewedTask];

    return (
      <>
      { task !== null && (
      <div className="popup-background">
          <div className="popup-window">
              <button type="button" className="btn-close popup-close" aria-label="Close" onClick={ () => { setTask(null) } }></button>
              <div className="popup-body">
                  
                    <h1>{task.TaskTitle}</h1>
                    <p>{task.TaskDescription}</p>
                    {/* <p>{task.taskType}</p> */}

                    {task.TaskType == TaskType.SocialMedia && <SocialMedia/> }
                    {task.TaskType == TaskType.ImageUpload && <ImageUpload/> }
                    {task.TaskType == TaskType.VideoUpload && <VideoUpload/> }
                    {task.TaskType == TaskType.Quiz && <Quiz/> }
                    {task.TaskType == TaskType.Contest && <p>Contest goes here</p> }
                  
              </div>
          </div>
          </div>
          ) }
      </>
  );
}

export default TaskWindow;