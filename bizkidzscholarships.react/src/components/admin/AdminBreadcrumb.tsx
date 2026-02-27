import { Children } from "react";
import { Link } from "react-router-dom";

type Props = {
    children: React.ReactNode;
};

function AdminBreadcrumb({ children }: Props) {
    return (<>
        <Link to=".." className="d-block">← Back to Admin Dashboard</Link>
      {children}
    </>);
}

export default AdminBreadcrumb;