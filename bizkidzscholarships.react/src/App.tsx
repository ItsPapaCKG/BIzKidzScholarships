import { useEffect, createContext } from 'react'
import './App.css'
import Dashboard from './components/dashboard/DashboardComponent';
import { useNavigate } from 'react-router-dom';
import { UseUserAccountContext } from './contexts/UserAccountContext';
import { AppMode, type UserCookieJSON } from './models/ViewModels';
import AdminDashboard from './components/admin/AdminDashboard';
import AdminProvider from './contexts/AdminContext';
import AuthFormComponent from './components/AuthFormComponent';

export const IsNewAccount = createContext(false);



interface AppProps {
    Mode: AppMode
}

function App({ Mode }: AppProps) {
    const navigate = useNavigate();
    const userAccountContext = UseUserAccountContext();
    const isAuthenticated = userAccountContext.isAuthenticated;
    const [setUserCookie] = [userAccountContext.setUserCookie];

    const check = async () => {
        var res = await fetch("https://localhost:7095/auth/me", { credentials: "include" });

        if (!res.ok) {
            navigate("/login")
            return;
        }

        var cookie = await res.json() as UserCookieJSON;

        setUserCookie(cookie);
        userAccountContext.setIsAuthenticated(true);
    }

    useEffect(() => {
        if (Mode == AppMode.Login || Mode == AppMode.Register) {
            return;
        }

        var ac = new AbortController();
        check();

        return () => ac.abort()

    }, []);

    return (
        <>
        <div className="d-flex flex-column min-vh-100">
            <nav className='navbar navbar-expand-lg navbar-light bg-light fixed-top'>
                <div id="website-logo">
                    <img src="https://assets.zyrosite.com/cdn-cgi/image/format=auto,w=192,h=192,fit=crop,f=png/mp86LE4kBWs8n2nr/bizkidzusa-logo-YZ97oQKGGyhz1EMk.png"/>
                </div>
            </nav>

            <main className="flex-grow-1 d-flex justify-content-center align-items-center">
                {isAuthenticated && Mode == AppMode.Dashboard && (<Dashboard/>)}

                {isAuthenticated && Mode == AppMode.Admin && (<AdminProvider><AdminDashboard /></AdminProvider>)}

                {!isAuthenticated && Mode == AppMode.Login && (<>
                    <AuthFormComponent />
                </>)}

                {!isAuthenticated && Mode == AppMode.Register && (<>
                    <AuthFormComponent RegisterMode={true}/>
                </>)}
            </main>
        </div>
        </>
    );
}

export default App

