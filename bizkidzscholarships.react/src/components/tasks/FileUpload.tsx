import { useRef, useState } from "react";
import { ProfileUpload, TaskUpload } from "../../services/UserDataService"
import { UseTaskContext } from "../../contexts/TaskViewContext";
import { ActionType } from "../../models/ViewModels";

interface ImageUploadProps {
    action: ActionType,
    setFileUrl?: React.Dispatch<React.SetStateAction<string>>
}

function FileUpload({ action, setFileUrl }: ImageUploadProps) {
    let fileUploadRef = useRef<HTMLInputElement>(null);

    const [statefulFile, setCurrentFile] = useState<File | undefined>(undefined);

    let uploadClick = () => {
        fileUploadRef.current!.click();
    }

    const UploadFile = async (file?: File) => {
        if (file) {
            setCurrentFile(file);
        }

        let focusedFile = file ?? statefulFile;

        if (focusedFile == undefined) {
            alert("Invalid upload.") 
            return false;
        }
        
        if (task?.TaskId == null && action == ActionType.TaskUpload) {
            return;
        }

        setError("");
        let successful = false;

        switch (action) {
            case ActionType.ProfileImageUpload:
                let response = await ProfileUpload(focusedFile);

                if (!response.Success || response.Url == null)
                {
                    break;
                }

                if (setFileUrl != null)
                    setFileUrl(response.Url);
                
                break;
            case ActionType.TaskUpload:
                successful = await TaskUpload(task!.TaskId, focusedFile);

                if (successful) {
                    setTask(null);
                    setTaskRefresh(true);
                }
        }

        setError("Error Uploading File. Please try again.");
    }

    const viewedTask = UseTaskContext();
    const [task, setTask] = [viewedTask.viewedTask, viewedTask.setViewedTask];
    const [setTaskRefresh] = [viewedTask.setTaskRefresh];
    const [error, setError] = useState<string>("");

    return (
      <>
        {action == ActionType.TaskUpload && (
            <>
                <label className="upload-label">
                
                    <input type="file" ref={ fileUploadRef } style={{ display: "none" } } onChange={(e) => { setCurrentFile(e.target.files?.[0]) } }/>
                    <button type="button" className="upload-btn" onClick={uploadClick }>Upload File</button>
                    {statefulFile && ( 
                        <>
                            <p>{ statefulFile.name }</p> 
                            <button type="submit" className="submit-btn" onClick={() => UploadFile() }>Submit</button>
                        </>
                    )}
                    
                </label>

            <p>{ error }</p>
        </>
        )}

        {action == ActionType.ProfileImageUpload && (
            <>
                    <input type="file" ref={fileUploadRef} style={{ display: "none" }} onChange={(e) => { UploadFile(e.target.files?.[0]); } }/>
                    <button type="button" className="upload-btn" onClick={uploadClick }>Upload File</button>
                    {statefulFile && (<p>{ statefulFile.name }</p>)} 
            </>
        )}
      </>
  );
}

export default FileUpload;