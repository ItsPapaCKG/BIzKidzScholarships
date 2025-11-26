
export enum UserTaskStatus {
    Hidden = -3,
    Disabled = -2,
    Rejected = -1,
    Open,
    Pending,
    Completed
}

export enum TaskType {
    SocialMedia,
    ImageUpload,
    VideoUpload,
    Quiz,
    Contest
}

export interface IUserProfile {
    BusinessName: string,
    FirstName: string,
    LastName: string,
    BusinessEmail: string,
    PhoneNumber: string,
    BusinessLogoKey: string
}

export interface ITask {
    taskTitle: string,
    taskDescription: string,
    reward: number,
    status: UserTaskStatus,
    taskImageKey: string,
    taskId: number,
    TaskType: TaskType
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

export interface PresignedURLData {
    url: string,
    key: string,
    fields: { [key: string]: string }
}

export default '.'