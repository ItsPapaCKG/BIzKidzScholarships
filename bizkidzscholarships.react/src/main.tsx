import { StrictMode, useContext } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import App from './App.tsx'
import { BrowserRouter as Router, Route, createBrowserRouter, RouterProvider } from 'react-router-dom'
import LoginComponent from './components/Login.tsx'

export const router = createBrowserRouter([
    { path: "/", element: <App/> },
    { path: "/login", element: <LoginComponent /> }
]);

createRoot(document.getElementById('root')!).render(
  <StrictMode>
        <RouterProvider router={router } />
  </StrictMode>,
)
