import { UseAdminContext } from "../../contexts/AdminContext";

function UserActivity() {
    const context = UseAdminContext();

    const [userActivities] = [context.userActivities]; 

    return (
        <>
            <table className="table table-light table-striped">
                <thead>
                    <tr>
                        <td>Name</td>
                        <td>Task</td>
                        <td></td>
                        <td>Completed</td>
                    </tr>
                </thead>
                <tbody>
                    {
                        userActivities.map((v)=>{
                            let isGain = v.Reward > 0;

                            return (<tr><td>{v.FullName}</td><td>{v.TaskName}</td><td className="text-nowrap" style={{ "width": "1px"}}><p className={(isGain ? "text-success" : "text-danger") + " m-0"}>{ isGain ? "+" : "-"}{v.Reward}</p></td><td>{v.ActivityDateTime.toString()}</td></tr>)
                        })
                    }
                </tbody>
            </table>
        </>
    );
}

export default UserActivity;