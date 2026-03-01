import { UseAdminContext } from "../../contexts/AdminContext";
import { UserType } from "../../models/ViewModels";

function UserList() {
    const context = UseAdminContext();

    const [userResults] = [context.userResults];

    return (
        <>
            <div className="container">
                <div className="row">
                    <div className="col">
                        <div className="table-responsive">
                            <table className="table table-light table-striped w-100">
                                <thead>
                                    <tr>
                                        <th>Kid Name</th>
                                        <th>Parent Name</th>
                                        <th>Email</th>
                                        <th>Points</th>
                                        <th>Entries</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {
                                        userResults.map((v) => {

                                            return (
                                                <tr>
                                                    <td>{v.UserType != UserType.KidOverThirteen ? (v.ChildFullName ?? "-" ) : "-"}</td>
                                                    <td>
                                                        {v.UserType == UserType.Parent &&
                                                            <span>
                                                                {/*<strong>Parent Name: </strong>*/}
                                                                {v.Name}
                                                            </span>
                                                        || "-"}
                                                    </td>
                                                    <td>{v.Email ?? "-" }</td>
                                                    <td>{v.Points}</td>
                                                    <td>{v.Entries}</td>
                                                </tr>)
                                        })
                                    }
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>

            </div>

           
        </>
    );
}

export default UserList;