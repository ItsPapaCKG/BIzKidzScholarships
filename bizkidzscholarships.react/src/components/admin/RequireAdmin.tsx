import { Navigate, useLocation } from "react-router-dom";
import { UseUserAccountContext, type userAccountContextType } from "../../contexts/UserAccountContext";

type Props = {
    children: React.ReactNode;
};

export default function RequireAdmin({ children }: Props) {
    const context: userAccountContextType = UseUserAccountContext();

    const location = useLocation();

    if (context.loadingData) {
        return <div>Loading...</div>; // or spinner
    }

    if (!context.isAuthenticated) {
        // Not logged in > send to login
        return <Navigate to="/login" state={{ from: location }} replace />;
    }

    if (!context.isAdmin) {
        // Logged in but not admin > send somewhere safe
        return <Navigate to="/" replace />;
    }

    // ✅ Allowed
    return <>{children}</>;
}