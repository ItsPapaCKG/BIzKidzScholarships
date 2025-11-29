import { useRef, useState } from "react";
import { TaskUpload } from "../../services/UserDataService"
import { UseTaskContext } from "../../contexts/TaskViewContext";
import { ActionType } from "../../models/ViewModels";

interface ImageUploadProps {
    action: ActionType
}

function ImageUpload({ action }: ImageUploadProps) {
    let fileUploadRef = useRef<HTMLInputElement>(null);

    const [currentFile, setCurrentFile] = useState<File | undefined>(undefined);

    let uploadClick = () => {
        fileUploadRef.current!.click();
    }

    const UploadFile = async () => {
        if (currentFile == undefined) {
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
                break;
            case ActionType.TaskUpload:
                successful = await TaskUpload(task!.TaskId, currentFile);
        }
        

        if (successful) {
            setTask(null);
            setTaskRefresh(true);
        }

        setError("Error Uploading File. Please try again.");
    }

    const viewedTask = UseTaskContext();
    const [task, setTask] = [viewedTask.viewedTask, viewedTask.setViewedTask];
    const [setTaskRefresh] = [viewedTask.setTaskRefresh];
    const [error, setError] = useState<string>("");

    return (
      <>
        <label className="upload-label">
            {/* <input type="file" ref={ fileUploadRef } style={{ display: "none" } } onChange={(e) => { TaskUploadChange(e.target.files?.[0])} }/> */}
            <input type="file" ref={ fileUploadRef } style={{ display: "none" } } onChange={(e) => { setCurrentFile(e.target.files?.[0]) } }/>
            <button type="button" className="upload-btn" onClick={uploadClick }>Upload File</button>
            {currentFile && ( 
                <>
                    <p>{ currentFile.name }</p> 
                    <button type="submit" className="submit-btn" onClick={ UploadFile }>Submit</button>
                </>
            )}
            
        </label>

        <p>{ error }</p>
      </>
  );
}

export default ImageUpload;