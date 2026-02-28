import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import App from './App.tsx'
import { createBrowserRouter, RouterProvider } from 'react-router-dom'
import AuthFormComponent from './components/AuthFormComponent.tsx'
import UserAccountProvider from './contexts/UserAccountContext.tsx'
import TaskProvider from './contexts/TaskViewContext.tsx'
import { AppMode } from './models/ViewModels.ts'
import "bootstrap/dist/css/bootstrap.min.css";
import AdminDashboard from './components/admin/AdminDashboard.tsx'
import TaskSubmissionsShell from './components/admin/TaskSubmissionsShell.tsx'
import AdminBreadcrumb from './components/admin/AdminBreadcrumb.tsx'
import PasswordReset from './components/auth/PasswordReset.tsx'
import PasswordResetConfirm from './components/auth/PasswordResetConfirm.tsx'
import UserList from './components/admin/UserList.tsx'

export const router = createBrowserRouter([
    { path: "/access-denied", element: (<><h1>Access is Denied Loser!</h1></>) },
    { path: "/", element: <App Mode={AppMode.Dashboard}/> },
    { path: "/login", element: <App Mode={AppMode.Login}/> },
    { path: "/register", element: <App Mode={AppMode.Register} /> },
    { path: "/passwordreset", element: <App Mode={AppMode.ForgotPassword} /> },
    { path: "/passwordresetconfirm", element: <App Mode={AppMode.ResetPassword} /> },
    { path: "/logout", element: <App Mode={ AppMode.Logout } /> },
    { path: "/privacy", element: <App Mode={ AppMode.Privacy } /> },
    { path: "/terms", element: <App Mode={ AppMode.Terms } /> },
    {
        path: "/admin",
        element: <App Mode={AppMode.Admin} />,
        children: [
            {
                index: true,
                element: <AdminDashboard />
            },
            {
                path: "users",
                element: <AdminBreadcrumb><UserList /></AdminBreadcrumb>
            },
            {
                path: "submissions",
                element: <AdminBreadcrumb><TaskSubmissionsShell /></AdminBreadcrumb>
            },
            {
                path: "tasks",
                element: <AdminBreadcrumb><p>Tasks configurations are a WIP</p></AdminBreadcrumb>
            }
        ]
    }
]);

createRoot(document.getElementById('root')!).render(
    <StrictMode>
        <UserAccountProvider>
            <TaskProvider>
                <RouterProvider router={router} />
            </TaskProvider>
        </UserAccountProvider>
  </StrictMode>
)
