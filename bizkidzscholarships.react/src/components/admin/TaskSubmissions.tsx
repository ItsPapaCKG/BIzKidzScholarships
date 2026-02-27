import { useEffect, useState } from "react";
import { UseAdminContext } from "../../contexts/AdminContext";
import { TaskType, type SubmissionItemJSON, type SubmissionsSearchResults } from "../../models/ViewModels";

export interface TaskSubmissionProps {
    taskId: number
}

function TaskSubmissions({ taskId }: TaskSubmissionProps) {
    const adminContext = UseAdminContext();
    const [submissions] = [adminContext.taskSubmissions]
    const [viewedSubmissions, setViewedSubmissions] = useState <SubmissionItemJSON[] | undefined>()
    const [GetSubmissions] = [adminContext.getTaskSubmissions];
    const [GetTasks] = [adminContext.getTasks];

    const filterSubmissions = (taskId: number) => {
        let filtered = submissions?.results.filter(item => item.taskId == taskId);

        setViewedSubmissions(filtered);
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
                    
                        <table className="table table-hover">
                            <thead>
                                <tr>
                                    <th>Submitted By</th>
                                    <th>Submission Type</th>
                                    <th>File Link</th>
                                    <th>Submitted Date</th>
                                </tr>
                            </thead>
                            <tbody>
                                {viewedSubmissions!.map((sub, idx) => {
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
                                        <td>{isFileType ? (<button type="button" className="btn btn-link">View File</button>) : <></>}</td>
                                        <td>{ sub.created.toISOString() }</td>
                                    </tr>)
                                }) }
                            </tbody>
                        </table>
                    
                
            </div>)}
            </>
    );
}

export default TaskSubmissions;