import { useRef } from "react";
import { TaskUploadChange } from "../../services/UserDataService"

function ImageUpload() {
    let fileUploadRef = useRef<HTMLInputElement>(null);

    let uploadClick = () => {
        fileUploadRef.current!.click();
    }

    return (
      <>
      <div className="popup-background">
          <div className="popup-window">
              <button type="button" className="btn-close popup-close" aria-label="Close"></button>
              <div className="popup-body">
                  <p>Upload file</p>
                  <label className="upload-label">
                            <input type="file" ref={ fileUploadRef } style={{ display: "none" } } onChange={(e) => { TaskUploadChange(e.target.files?.[0])} }/>
                            <button type="button" className="upload-btn" onClick={uploadClick }>Upload File</button>
                  </label>
              </div>
          </div>
          </div>
      </>
  );
}

export default ImageUpload;