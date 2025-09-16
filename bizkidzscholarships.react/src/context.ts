import { createContext, useContext } from 'react';
import type { IUserPoints, IUserProfile, ITask } from './models/ViewModels';

export const UserProfileContext = createContext<IUserProfile | undefined>(undefined);
export const UserTasksContext = createContext<ITask[] | undefined>(undefined);
export const UserPointsContext = createContext<IUserPoints | undefined>(undefined);

export function useUserProfileContext() {
    const profile = useContext(UserProfileContext);

    if (profile == undefined) {
        throw new Error('useUserProfileContext must be used within the UserProfileContext');
    }

    return profile;
}

export function useTasksContext() {
    const tasks = useContext(UserTasksContext);

    if (tasks == undefined) {
        throw new Error('useTasksContext must be used within the UserTasksContext');
    }

    return tasks;
}

export function useUserPointsContext() {
    const points = useContext(UserPointsContext);

    if (points == undefined) {
        throw new Error('useTasksContext must be used within the UserTasksContext');
    }

    return points;
}