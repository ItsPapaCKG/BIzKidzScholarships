import { useRef } from "react";
import { TaskUploadChange } from "../../services/UserDataService"
import { UseViewedTaskContext } from "../../contexts/TaskViewContext";

function ImageUpload() {
    let fileUploadRef = useRef<HTMLInputElement>(null);

    let uploadClick = () => {
        fileUploadRef.current!.click();
    }

    const viewedTask = UseViewedTaskContext();
    const [task, setTask] = [viewedTask.viewedTask, viewedTask.setViewedTask];

    return (
      <>
        <label className="upload-label">
            <input type="file" ref={ fileUploadRef } style={{ display: "none" } } onChange={(e) => { TaskUploadChange(e.target.files?.[0])} }/>
            <button type="button" className="upload-btn" onClick={uploadClick }>Upload File</button>
        </label>
      </>
  );
}

export default ImageUpload;