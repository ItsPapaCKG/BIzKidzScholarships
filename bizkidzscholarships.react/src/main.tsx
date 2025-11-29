import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import App from './App.tsx'
import { createBrowserRouter, RouterProvider } from 'react-router-dom'
import LoginComponent from './components/Login.tsx'
import UserAccountProvider from './contexts/UserAccountContext.tsx'
import TaskProvider from './contexts/TaskViewContext.tsx'

export const router = createBrowserRouter([
    { path: "/", element: <App/> },
    { path: "/login", element: <LoginComponent /> }
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
