import { UseAdminContext } from "../../contexts/AdminContext";

function UserList() {
    const context = UseAdminContext();

    const [userResults] = [context.userResults];

    return (
        <>
            <table className="table table-light table-striped">
                <thead>
                    <tr>
                        <td>Name</td>
                        <td>Points</td>
                        <td>Entries</td>
                    </tr>
                </thead>
                <tbody>
                    {
                        userResults.map((v)=>{

                            return (<tr><td>{v.Name}</td><td>{v.Points}</td><td>{v.Entries}</td></tr>)
                        })
                    }
                </tbody>
            </table>
        </>
    );
}

export default UserList;