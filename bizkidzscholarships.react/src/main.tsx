import { StrictMode, useContext } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import App from './App.tsx'
import { BrowserRouter as Router, Route, createBrowserRouter, RouterProvider } from 'react-router-dom'
import LoginComponent from './components/Login.tsx'
import UserAccountProvider from './contexts/UserAccountContext.tsx'

export const router = createBrowserRouter([
    { path: "/", element: <App/> },
    { path: "/login", element: <LoginComponent /> }
]);

createRoot(document.getElementById('root')!).render(
    <StrictMode>
        <UserAccountProvider>
            <RouterProvider router={router} />
        </UserAccountProvider>
  </StrictMode>,
)
