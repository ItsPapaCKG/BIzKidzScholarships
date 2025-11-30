
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

export enum AppMode {
    Dashboard,
    Admin
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
    TaskType: TaskType,
    TaskPromptTitle: string,
    TaskPromptSubtitle: string
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

export interface StartUploadHandshakeResponseJSON {
    requestId: string,
    presignedUrlPayload: PresignedURLData
}

export interface ServerUploadResponse {
    Success: boolean,
    Url?: string
}

export enum RequestStatus
{
    Denied = -1,
    Cancelled,
    Pending,
    Closed,
    Success,
    Failed
}

export interface StartUploadRequest {
    ActionType: ActionType,
    TaskId?: Number,
    Extension: string
}

export interface UploadHandshakeConfirmation {
    RequestId: string,
    Status: RequestStatus
}

export interface UserActivityLog {
    FullName: string,
    TaskName: string,
    Reward: number,
    ActivityDateTime: Date
}

export default '.'