import React, { useState, Component, useEffect } from 'react'
import './App.css'
import { UserPointsContext, UserProfileContext, UserTasksContext } from './context';
import type { IUserProfile, ITask, IUserPoints } from './models/ViewModels';
import { UserTaskStatus } from './models/ViewModels';
import Dashboard from './components/DashboardComponent';
import { useNavigate } from 'react-router-dom';

function App({ }) {
    var navigate = useNavigate();
    var [isAuthenticated, setIsAuthenticated] = useState(false);

    const check = async () => {
        var res = await fetch("https://localhost:7095/auth/me", { credentials: "include" });

        if (res.ok)
            return true;

        return false;
    }

    useEffect(() => {

        var ac = new AbortController();

        (async () => {
            var loggedIn = await check();

            if (!loggedIn)
                navigate("/login");
        })();

        return () => ac.abort()

    }, []);

    return (
        <Dashboard />
    );
}

export default App
