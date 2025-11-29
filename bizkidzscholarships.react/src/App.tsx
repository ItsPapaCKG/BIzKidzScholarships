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

        if (!res.ok) {
            navigate("/login")
            return;
        }

        userAccountContext.setIsAuthenticated(true);
    }

    useEffect(() => {
        var ac = new AbortController();
        check();

        return () => ac.abort()

    }, []);

    return (
        <>
            <nav className='top-bar'>
                <div id="website-logo">
                    <img src="https://assets.zyrosite.com/cdn-cgi/image/format=auto,w=192,h=192,fit=crop,f=png/mp86LE4kBWs8n2nr/bizkidzusa-logo-YZ97oQKGGyhz1EMk.png"/>
                </div>
            </nav>

            <main>
                {isAuthenticated ? <Dashboard /> : <p>Loading...</p>}
            </main>
        </>
    );
}

export default App
