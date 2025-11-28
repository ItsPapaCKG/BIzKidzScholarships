
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

export enum ActionType {
    TaskUpload,
    ProfileImageUpload
}

export interface IUserProfile {
    BusinessName: string,
    FirstName: string,
    LastName: string,
    BusinessEmail: string,
    PhoneNumber: string,
    BusinessLogoKey: string
}

export interface ITaskJSON {
    taskTitle: string,
    taskDescription: string,
    reward: number,
    status: UserTaskStatus,
    taskImageKey: string,
    taskId: number,
    taskType: TaskType
}

export interface ITask {
    TaskTitle: string,
    TaskDescription: string,
    Reward: number,
    Status: UserTaskStatus,
    TaskImageKey: string,
    TaskId: number,
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

export interface StartUploadHandshakeResponse {
    RequestId: Number,
    PresignedData: PresignedURLData
}

export interface ServerResponse {

}

export interface StartUploadRequest {
    ActionType: ActionType,
    TaskId?: Number,
    Extension: string
}

export default '.'