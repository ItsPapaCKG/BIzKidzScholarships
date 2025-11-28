import { useRef, useState } from "react";
import { TaskUploadChange } from "../../services/UserDataService"
import { UseViewedTaskContext } from "../../contexts/TaskViewContext";

function ImageUpload() {
    let fileUploadRef = useRef<HTMLInputElement>(null);

    const [currentFile, setCurrentFile] = useState<File | undefined>(undefined);

    let uploadClick = () => {
        fileUploadRef.current!.click();
    }

    const UploadForTask = async () => {
        if (task?.TaskId == null) {
            return;
        }

        let successful = await TaskUploadChange(task.TaskId, currentFile);

        if (successful) {
            setTask(null);
        }

        setError("Error Uploading File. Please try again.");
    }

    const viewedTask = UseViewedTaskContext();
    const [task, setTask] = [viewedTask.viewedTask, viewedTask.setViewedTask];
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
                    <button type="submit" className="submit-btn" onClick={ UploadForTask }>Submit</button>
                </>
            )}
            
        </label>

        <p>{ error }</p>
      </>
  );
}

export default ImageUpload;