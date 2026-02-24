import { Outlet } from "react-router-dom";
import RequireAdmin from "./RequireAdmin";

function AdminGuard() {
  return (
      <RequireAdmin>
        <Outlet />
      </RequireAdmin>
  );
}

export default AdminGuard;