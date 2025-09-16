
export enum UserTaskStatus {
    Hidden = -3,
    Disabled = -2,
    Rejected = -1,
    Open,
    Pending,
    Completed
}

export interface IUserProfile {
    BusinessName: string,
    KidFullName: string,
    BusinessEmail: string,
    BusinessPhone: string,
    BusinessLogoURL: string
}

export interface ITask {
    Title: string,
    Description: string,
    Points: number,
    Status: UserTaskStatus,
    TaskImageURL: string,
    TaskId: number
}

export interface IUserPoints {
    Total: number,
    Entries: number
}

export interface IDashboardContext {
    UserProfile: IUserProfile,
    UserPoints: IUserPoints
    Tasks: ITask[]
}

export default '.'