import { useEffect } from "react";
import { UseAdminContext } from "../../../contexts/AdminContext";

function UsersAdminList() {
    const context = UseAdminContext();



    useEffect(() => { }, []);

    return (
        <>
            <div className="table-responsive">
                <table className="table">
                    <thead>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
            </div>
        </>
  );
}

export default UsersAdminList;