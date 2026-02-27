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

export const router = createBrowserRouter([
    { path: "/access-denied", element: (<><h1>Access is Denied Loser!</h1></>) },
    { path: "/", element: <App Mode={AppMode.Dashboard}/> },
    { path: "/login", element: <App Mode={AppMode.Login}/> },
    { path: "/register", element: <App Mode={AppMode.Register}/> },
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
                element: <p>Users go here</p>
            },
            {
                path: "submissions",
                element: <AdminBreadcrumb><TaskSubmissionsShell /></AdminBreadcrumb>
            },
            {
                path: "tasks",
                element: <p>Tasks go here</p>
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
