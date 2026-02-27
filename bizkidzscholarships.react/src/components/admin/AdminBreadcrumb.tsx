import { Children } from "react";
import { Link } from "react-router-dom";

type Props = {
    children: React.ReactNode;
};

function AdminBreadcrumb({ children }: Props) {
    return (<div className="container">
        <div className="row">
            <Link to=".." className="d-block">← Back to Admin Dashboard</Link>
        </div>
        <div className="row">
            {children}
        </div>
    </div>);
}

export default AdminBreadcrumb;