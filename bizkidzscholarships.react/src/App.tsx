import React, { useState, Component } from 'react'
import { BrowserRouter as Router, Route  } from 'react-router-dom'
import './App.css'
import { UserPointsContext, UserProfileContext, UserTasksContext } from './context';
import type { IUserProfile, ITask, IUserPoints } from './models/ViewModels';
import { UserTaskStatus } from './models/ViewModels';
import Dashboard from './components/DashboardComponent';

function App({ }) {
    const [userProfile, setUserProfile] = useState<IUserProfile>({
        BusinessName: "Element Pressure Washing SWFL",
        BusinessEmail: "grant@elementswfl.com",
        BusinessPhone: "123456789",
        KidFullName: "Grant Shaun Putnam",
        BusinessLogoURL: "https://tse1.mm.bing.net/th/id/OIP.7TsQHOGwjUs-ztCTbB43XwHaEj?r=0&rs=1&pid=ImgDetMain&o=7&rm=3"
    });

    const [tasks, setTasks] = useState<ITask[]>([
        {
            TaskId: 4,
            TaskImageURL: "www.google.com",
            Description: "Complete the task and win points",
            Status: UserTaskStatus.Open,
            Title: "Task 1",
            Points: 10
        },
        {
            TaskId: 5,
            TaskImageURL: "www.google.com",
            Description: "Complete this other task and win points",
            Status: UserTaskStatus.Open,
            Title: "Task 2",
            Points: 20
        },
        {
            TaskId: 6,
            TaskImageURL: "www.google.com",
            Description: "Complete the coolest task to win even MORE points",
            Status: UserTaskStatus.Completed,
            Title: "Task 1",
            Points: 15
        }
    ])

    const [points, setPoints] = useState<IUserPoints>({
        Total: 50,
        Entries: 5
    });

    return (
        <UserProfileContext.Provider value={userProfile}>
            <UserTasksContext.Provider value={tasks}>
                <UserPointsContext value={points}>
                    <Dashboard />
                </UserPointsContext>
            </UserTasksContext.Provider>
        </UserProfileContext.Provider>
    );
}

export default App
