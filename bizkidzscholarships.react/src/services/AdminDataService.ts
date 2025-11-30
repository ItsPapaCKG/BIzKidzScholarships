import type { UserActivityLog } from "../models/ViewModels";

export async function GetUserActivities(): Promise<UserActivityLog[]> {
    // TODO send a request to get the UserActivityView

    let logs = [
        { FullName: "Grant Putnam", TaskName: "Business Photo Upload", Reward: 10, ActivityDateTime: new Date(2025, 11, 30, 18, 40, 0) } as UserActivityLog,
        { FullName: "Grant Putnam", TaskName: "Business Photo Upload 2", Reward: 10, ActivityDateTime: new Date(2025, 11, 30, 17, 40, 0) } as UserActivityLog,
        { FullName: "Grant Putnam", TaskName: "Miscellaneous Task 3", Reward: 10, ActivityDateTime: new Date(2025, 11, 29, 17, 40, 0) } as UserActivityLog
    ]

    await setTimeout(() => {}, 50);

    return logs;
}