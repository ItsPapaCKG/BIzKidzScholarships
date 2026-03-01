import { useEffect, useRef, useState } from "react";
import { ProfileUpload, SendUserConsent, TaskUpload } from "../../services/UserDataService"
import { UseTaskContext } from "../../contexts/TaskViewContext";
import { ActionType, BizDocumentType } from "../../models/ViewModels";
import LoadingSVG from "../LoadingSVG";

interface ImageUploadProps {
    action: ActionType,
    setFileUrl?: (url: string) => void,
    isVideo?: boolean,
    onClose: () => void
}

function FileUpload({ action, setFileUrl, isVideo, onClose }: ImageUploadProps) {
    let fileUploadRef = useRef<HTMLInputElement>(null);

    const isVideoSafe = isVideo ?? false;

    const [statefulFile, setCurrentFile] = useState<File | undefined>(undefined);
    const [previewUrl, setPreviewUrl] = useState<string | null>(null);
    const [uploadDisabled, setUploadDisabled] = useState<boolean>(true);
    const [uploadPending, setUploadPending] = useState<boolean>(false);

    let uploadClick = () => {
        fileUploadRef.current!.click();
    }

    const UploadFile = async (file?: File) => {
        setUploadDisabled(true);

        if (file) {
            setCurrentFile(file);
        }

        let focusedFile = file ?? statefulFile;

        if (focusedFile == undefined) {
            alert("Invalid upload.") 
            setUploadDisabled(false);
            return false;
        }
        
        if (task?.TaskId == null && action == ActionType.TaskUpload) {
            console.error("No task Id found for task. Could not upload submission.");
            setUploadDisabled(false);
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

                onClose();

                if (setFileUrl != null)
                    setFileUrl(response.Url);

                successful = true;
                break;
            case ActionType.TaskUpload:
                setUploadPending(true);
                let c = await SendUserConsent(BizDocumentType.MediaConsent, consent);

                if (!c.success) {
                    break;
                }

                successful = await TaskUpload(task!.TaskId, focusedFile, c.data?.consentId ?? 0);

                if (successful) {
                    onClose();

                    setUploadPending(false);

                    setTask(null);
                    setPointsRefresh(true);
                    setTaskRefresh(true);
                }
                break;
        }

        setUploadPending(false);
        setUploadDisabled(false);
        setError("Error Uploading File. Please try again.");
    }

    const toggleConsent = () => {
        setConsent(!consent);
    }

    const viewedTask = UseTaskContext();
    const [task, setTask] = [viewedTask.viewedTask, viewedTask.setViewedTask];
    const [setTaskRefresh] = [viewedTask.setTaskRefresh];
    const [error, setError] = useState<string>("");
    const [setPointsRefresh] = [viewedTask.setPointsRefresh];
    const [consent, setConsent] = useState<boolean>(false);

    useEffect(() => {
        if (consent) {
            setUploadDisabled(false);
            return;
        }

        setUploadDisabled(true);
    }, [consent]);

    return (
        <>
            

                {action == ActionType.TaskUpload && (
                    <>
                    
                    <div className="row" style={{ minHeight: "40%" }}>
                        <div className="col m-auto">
                                {previewUrl ? (
                                isVideoSafe ? (<div className="ratio ratio-16x9"><video src={previewUrl} preload="metadata" controls className="object-cover w-100 h-100 taskWindowPreview d-md-block" controlsList="nodownload" playsInline /></div>) : (<img src={previewUrl} className="img-fluid w-100" style={{ maxHeight: "40vh", objectFit: "contain"  }} />)
                                ) : (
                                    <p className="text-gray-400 text-sm mx-auto text-center w-100">No media</p>
                                )}
                        </div>
                    </div>

                    { previewUrl && 
                    <div className="row">
                            <div className="form-check">
                                <label className="form-check-label">
                                    <input type="checkbox" className="form-check-input" name="MediaConsent" checked={consent} onChange={toggleConsent}></input>

                                    As a condition of participation, I grant permission for submitted materials to be used for evaluation, program administration, and promotional purposes.<span className="text-danger">*</span>
                                </label>
                        </div>

                            {uploadPending && <LoadingSVG />}

                            {statefulFile && (<button type="submit" disabled={uploadDisabled} className="btn btn-success w-100" onClick={() => UploadFile()}>Submit</button>)}

                        
                    </div>
                    }
                    <div className="row">
                        <div className="d-grid gap-2 col-8 mx-auto">
                            <label className="upload-label">

                                {!isVideoSafe && <input accept="image/*" type="file" ref={fileUploadRef} style={{ display: "none" }} onChange={(e) => { let file = e.target.files?.[0]; if (!file) return; setPreviewUrl(URL.createObjectURL(file)); setCurrentFile(file); }} />}
                                {isVideoSafe && <input accept="video/*" type="file" ref={fileUploadRef} style={{ display: "none" }} onChange={(e) => { let file = e.target.files?.[0]; if (!file) return; setPreviewUrl(URL.createObjectURL(file)); setCurrentFile(file); }} />}
                                <button type="button" className="btn btn-primary w-100" onClick={uploadClick}>Upload { previewUrl ? "Different" : "" } File</button>

                            </label>
                        </div>
                    </div>
                </>
                )}

                {action == ActionType.ProfileImageUpload && (
                    <>
                    <input type="file" name="BusinessLogoKey" className="form-control" aria-label="Sizing example input" aria-describedby="inputGroup-sizing-default" onChange={(e) => { let file = e.target.files?.[0];  if (!file) return; setPreviewUrl(URL.createObjectURL(file)); UploadFile(e.target.files?.[0]); } }/>
                    </>
                )}

            <div className="row">
                <p>{error}</p>
            </div>
      </>
  );
}

export default FileUpload;