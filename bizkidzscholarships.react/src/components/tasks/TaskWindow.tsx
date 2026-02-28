import { UseTaskContext } from "../../contexts/TaskViewContext";
import { ActionType, TaskType } from "../../models/ViewModels";
import FileUpload from "./FileUpload";
import Quiz from "./Quiz";
import SocialMedia from "./SocialMedia";

function TaskWindow() {

    const viewedTask = UseTaskContext();
    const [task, setTask] = [viewedTask.viewedTask, viewedTask.setViewedTask];

    return (
      <>
                <div className="modal modal-xl fade" id="taskModal" tabIndex={-1} aria-labelledby="TaskModal">
                    <div className="modal-dialog">
                        <div className="modal-content">
                        <div className="modal-header">
                            {task && <>
                                <h5 className="modal-title" id="exampleModalLabel">{task.TaskPromptTitle}</h5>
                                <button type="button" className="btn-close" aria-label="Close" data-bs-dismiss="modal"></button>
                            </>}
                                
                            </div>
                        <div className="modal-body d-flex justify-content-center p-5" style={{ maxHeight: "100vh", minHeight: "60vh" }}>
                            <div>
                                {task && <>
                                    <div className="row mb-5">
                                        <div className="col">
                                            <p className="fs-5 m-0">{task.TaskPromptSubtitle}</p>
                                        </div>
                                    </div>

                                        {task.TaskType == TaskType.SocialMedia && <SocialMedia />}
                                        {task.TaskType == TaskType.ImageUpload && <FileUpload action={ActionType.TaskUpload} />}
                                        {task.TaskType == TaskType.VideoUpload && <FileUpload action={ActionType.TaskUpload} isVideo={true} />}
                                        {task.TaskType == TaskType.Quiz && <Quiz />}
                                        {task.TaskType == TaskType.Contest && <p>Contest goes here</p>}
                                </>}
                            </div>
                            </div>
                        </div>
                    </div>
                </div>
      </>
  );
}

export default TaskWindow;