import { useEffect, createContext } from 'react'
import './App.css'
import Dashboard from './components/dashboard/DashboardComponent';
import { useNavigate } from 'react-router-dom';
import { UseUserAccountContext } from './contexts/UserAccountContext';
import { AppMode } from './models/ViewModels';
import AdminDashboard from './components/admin/AdminDashboard';

export const IsNewAccount = createContext(false);



interface AppProps {
    Mode: AppMode
}

function App({ Mode }: AppProps) {
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
                {isAuthenticated && Mode == AppMode.Dashboard && (<Dashboard/>)}

                {isAuthenticated && Mode == AppMode.Admin && (<AdminDashboard />)}
            </main>
        </>
    );
}

export default App
