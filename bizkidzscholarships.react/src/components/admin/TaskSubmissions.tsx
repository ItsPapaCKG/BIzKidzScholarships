import { useEffect, useState } from "react";
import { UseAdminContext } from "../../contexts/AdminContext";
import { TaskType, type GetSubmissionResponse, type SubmissionItemJSON } from "../../models/ViewModels";
import LoadingSVG from "../LoadingSVG";
import { GetSubmission } from "../../services/AdminDataService";

export interface TaskSubmissionProps {
    taskId: number
}

function TaskSubmissions({ taskId }: TaskSubmissionProps) {
    const adminContext = UseAdminContext();
    const [submissions] = [adminContext.taskSubmissions]
    const [viewedSubmissions, setViewedSubmissions] = useState <SubmissionItemJSON[] | undefined>()
    const [viewedSubmission, setViewedSubmission] = useState<SubmissionItemJSON | undefined>()
    const [modalLoading, setModalLoading] = useState<boolean>(false);
    const [modalError, setModalError] = useState<string | undefined>();
    const [submissionAccess, setSubmissionAccess] = useState<GetSubmissionResponse | undefined>()

    const filterSubmissions = (taskId: number) => {
        let filtered = submissions?.results.filter(item => item.taskId == taskId);

        setViewedSubmissions(filtered);
    }

    const closeModal = () => {
        setModalLoading(false);
        setModalError(undefined);
        setViewedSubmission(undefined);
        setSubmissionAccess(undefined);
    }

    const viewSubmission = async (submissionId: string) => {
        let item = viewedSubmissions?.find(i => i.submissionId == submissionId);

        setViewedSubmission(item);

        setModalLoading(true);

        let res = await GetSubmission(submissionId);

        if (!res.success) {
            setModalError(res.error.message);
            setModalLoading(false);
            return;
        }

        if (res.data?.s3Link == undefined) {
            setModalError("Could not retrieve S3 Link. Please contact your administrator.");
            setModalLoading(false);
            return;
        }

        setSubmissionAccess(res.data!);
        setModalLoading(false);
    }

    useEffect(() => {
        filterSubmissions(taskId);
    }, [taskId]);

    //SocialMedia,
    //    ImageUpload,
    //    VideoUpload,
    //    Quiz,
    //    Contest

    return (<>
        {!viewedSubmissions ?

            (<p>Loading...</p>) :

            (<div>
                    <div className="table-responsive">
                        <table className="table table-hover">
                            <thead>
                                <tr>
                                    <th>Submitted By</th>
                                    <th>Submission Type</th>
                                    <th></th>
                                    <th>Submitted Date</th>
                                </tr>
                            </thead>
                            <tbody>
                                {viewedSubmissions!.map((sub) => {
                                    {
                                        var taskTypeString: string;
                                        var isFileType = false;

                                        switch (sub.taskType) {
                                            case TaskType.SocialMedia:
                                                taskTypeString = "Social Media"
                                                break;
                                            case TaskType.ImageUpload:
                                                isFileType = true;
                                                taskTypeString = "Image"
                                                break;
                                            case TaskType.VideoUpload:
                                                isFileType = true;
                                                taskTypeString = "Video"
                                                break;
                                            case TaskType.Quiz:
                                                taskTypeString = "Quiz";
                                                break;
                                            case TaskType.Contest:
                                                taskTypeString = "Contest"
                                                break;
                                            default:
                                                taskTypeString = "";
                                        }
                                    } 

                                    return (<tr key={ sub.submissionId }>
                                        <td>{ sub.userFullName }</td>
                                        <td>{ taskTypeString }</td>
                                        <td>{isFileType ? (<button type="button" className="btn btn-link" onClick={() => viewSubmission(sub.submissionId)} data-bs-toggle="modal" data-bs-target="#viewSubmissionModal">View File</button>) : <></>}</td>
                                        <td>{ sub.created.toISOString() }</td>
                                    </tr>)
                                }) }
                            </tbody>
                        </table>
                    </div>
                
            </div>)}

                <div className="modal modal-xl fade" id="viewSubmissionModal" tabIndex={-1} aria-labelledby="exampleModalLabel">
                    <div className="modal-dialog">
                        <div className="modal-content">
                            <div className="modal-header">
                                <h5 className="modal-title" id="exampleModalLabel">{ viewedSubmission ? viewedSubmission.userFullName + "'s Submission" : "View Submission"}</h5>
                                <button type="button" className="btn-close" aria-label="Close" onClick={closeModal} data-bs-dismiss="modal"></button>
                            </div>
                            <div className="modal-body d-flex justify-content-center align-items-center" style={{ height: "75vh" }}>
                                {modalLoading && <div className="d-flex justify-content-center align-items-center" style={{ minHeight: "75vh" }}><LoadingSVG /></div>}

                                {modalError && <div className="d-flex justify-content-center align-items-center" style={{ minHeight: "75vh" }}>{modalError}</div>}

                        {submissionAccess?.s3Link && viewedSubmission?.taskType == TaskType.VideoUpload && <video controls src={submissionAccess?.s3Link} className="img-fluid" style={{ maxHeight: "75vh" }}></video>}
                        {submissionAccess?.s3Link && viewedSubmission?.taskType == TaskType.ImageUpload && <img src={submissionAccess?.s3Link} className="img-fluid" style={{ maxHeight: "75vh" }}></img>}
                            </div>
                            { submissionAccess?.s3Link && <div className="modal-footer">
                                <button type="button" className="btn btn-primary" onClick={async () => window.open(submissionAccess?.s3Link, "_blank")}>Download File</button>
                            </div>}
                        </div>
                    </div>
                </div>
            </>
    );
}

export default TaskSubmissions;