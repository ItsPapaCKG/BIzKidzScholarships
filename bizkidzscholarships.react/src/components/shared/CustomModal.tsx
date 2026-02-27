import type { PropsWithChildren } from "react";


function CustomModal({ children }: PropsWithChildren) {
  return (
      <div className="modal modal-xl fade show d-block" id="viewSubmissionModal" tabIndex={-1} aria-labelledby="exampleModalLabel" aria-hidden="true">
          <div className="modal-dialog">
              <div className="modal-content">
                  { children }
              </div>
          </div>
      </div>
  );
}

export default CustomModal;