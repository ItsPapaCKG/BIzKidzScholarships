import { useState, useEffect, createContext } from 'react'
import './App.css'
import Dashboard from './components/DashboardComponent';
import { useNavigate } from 'react-router-dom';
import { UseUserAccountContext } from './contexts/UserAccountContext';

export const IsNewAccount = createContext(false);

function App({ }) {
    const navigate = useNavigate();
    const userAccountContext = UseUserAccountContext();
    const isAuthenticated = userAccountContext.isAuthenticated;

    const check = async () => {
        var res = await fetch("https://localhost:7095/auth/me", { credentials: "include" });

        if (!res.ok)
            navigate("/login")

        userAccountContext.setIsAuthenticated(true);
    }

    useEffect(() => {
        var ac = new AbortController();
        check();

        return () => ac.abort()

    }, []);

    return (
        <>
            {isAuthenticated ? < Dashboard /> : <p>Loading...</p>}
        </>
    );
}

export default App
