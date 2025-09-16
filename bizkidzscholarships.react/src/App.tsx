import React, { useState, Component } from 'react'
import './App.css'
import { UserPointsContext, UserProfileContext, UserTasksContext } from './context';
import type { IUserProfile, ITask, IUserPoints } from './models/ViewModels';
import { UserTaskStatus } from './models/ViewModels';
import Dashboard from './components/DashboardComponent';

function App({ }) {
    

    return (
        <Dashboard />
    );
}

export default App
