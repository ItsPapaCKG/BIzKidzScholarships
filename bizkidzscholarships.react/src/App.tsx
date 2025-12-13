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
    const [cookie, setUserCookie] = [userAccountContext.userCookie, userAccountContext.setUserCookie];
    const populateCookie = userAccountContext.populateCookie;
    
    const check = async () => {
        var ac = new AbortController();

        ; await populateCookie();

        

        return () => ac.abort()
    }

    useEffect(() => {
        check();
    }, []);

    useEffect(() => {
        if (!cookie.email) {
            navigate('/login')
            return;
        }

        navigate("/");
    }, [cookie]);

    return (
        <>
        <div className="d-flex flex-column min-vh-100 bg-light">
            <nav className='navbar navbar-expand-lg navbar-light bg-white fixed-top h-10 shadow-lg'>
                <div className='container-xl'>
                    <a className='navbar-brand ms-5'>
                        <img className="navbarLogo" src="https://assets.zyrosite.com/cdn-cgi/image/format=auto,w=192,h=192,fit=crop,f=png/mp86LE4kBWs8n2nr/bizkidzusa-logo-YZ97oQKGGyhz1EMk.png" height="100"/>
                    </a>
                </div>
                
            </nav>

            <main className="flex-grow-1 d-flex justify-content-center container-xl h-100 mt-5">
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

