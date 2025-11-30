import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import App from './App.tsx'
import { createBrowserRouter, RouterProvider } from 'react-router-dom'
import LoginComponent from './components/Login.tsx'
import UserAccountProvider from './contexts/UserAccountContext.tsx'
import TaskProvider from './contexts/TaskViewContext.tsx'
import { AppMode } from './models/ViewModels.ts'

export const router = createBrowserRouter([
    { path: "/access-denied", element: (<><h1>Access is Denied Loser!</h1></>) },
    { path: "/", element: <App Mode={AppMode.Dashboard}/> },
    { path: "/login", element: <LoginComponent /> },
    { path: "/admin", element: <App Mode={AppMode.Admin}/>}
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
