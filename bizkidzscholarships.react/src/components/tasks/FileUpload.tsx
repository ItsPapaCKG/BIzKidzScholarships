import { useRef, useState } from "react";
import { ProfileUpload, TaskUpload } from "../../services/UserDataService"
import { UseTaskContext } from "../../contexts/TaskViewContext";
import { ActionType } from "../../models/ViewModels";

interface ImageUploadProps {
    action: ActionType,
    setFileUrl?: (url: string) => void,
    isVideo?: boolean
}

function FileUpload({ action, setFileUrl, isVideo }: ImageUploadProps) {
    let fileUploadRef = useRef<HTMLInputElement>(null);

    const isVideoSafe = isVideo ?? false;

    const [statefulFile, setCurrentFile] = useState<File | undefined>(undefined);
    const [previewUrl, setPreviewUrl] = useState<string | null>(null);

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
                    setPointsRefresh(true);
                    setTaskRefresh(true);
                }
        }

        setError("Error Uploading File. Please try again.");
    }

    const viewedTask = UseTaskContext();
    const [task, setTask] = [viewedTask.viewedTask, viewedTask.setViewedTask];
    const [setTaskRefresh] = [viewedTask.setTaskRefresh];
    const [error, setError] = useState<string>("");
    const [setPointsRefresh] = [viewedTask.setPointsRefresh];

    return (
      <>
        {action == ActionType.TaskUpload && (
            <>

                <label className="upload-label">
                
                    {!isVideoSafe && <input accept="image/*" type="file" ref={ fileUploadRef } style={{ display: "none" } } onChange={(e) => { let file = e.target.files?.[0];  if (!file) return; setPreviewUrl(URL.createObjectURL(file)); setCurrentFile(file); } }/>}
                    {isVideoSafe && <input accept="video/*" type="file" ref={ fileUploadRef } style={{ display: "none" } } onChange={(e) => { let file = e.target.files?.[0];  if (!file) return; setPreviewUrl(URL.createObjectURL(file)); setCurrentFile(file); } }/>}
                    <button type="button" className="btn btn-light btn-lg border-black" onClick={uploadClick }>Upload File</button>
                    
                </label>

                {statefulFile && ( 
                        <>
                            <p className="fs-5">{ statefulFile.name }</p> 
                        </>
                    )}

                <div className="m-auto" style={{ width: "70%" }}>
                    {previewUrl ? (
                        <img src={previewUrl} className="object-cover w-100 h-100 taskWindowPreview d-none d-md-block" />
                    ) : (
                        <span className="text-gray-400 text-sm m-auto text-center w-100">No image</span>
                    )}
                </div>

                {statefulFile && (<button type="submit" className="btn btn-lg btn-success" onClick={() => UploadFile() }>Submit</button>)}

            <p>{ error }</p>
        </>
        )}

        {action == ActionType.ProfileImageUpload && (
            <>
                    <input type="file" name="BusinessLogoKey" className="form-control" aria-label="Sizing example input" aria-describedby="inputGroup-sizing-default" onChange={(e) => { let file = e.target.files?.[0];  if (!file) return; setPreviewUrl(URL.createObjectURL(file)); UploadFile(e.target.files?.[0]); } }/>
            </>
        )}
      </>
  );
}

export default FileUpload;