
// #region UserUX
export interface IUserProfile {
    BusinessName: string,
    FirstName: string,
    LastName: string,
    Email: string,
    PhoneNumber: string,
    BusinessLogoKey: string,
    Birthday: Date,
    Loaded: boolean
}

export interface IDashboardContext {
    UserProfile: IUserProfile,
    UserPoints: IUserPoints
    Tasks: ITask[]
}
// #endregion

// #region Tasks
export enum ActionType {
    TaskUpload,
    ProfileImageUpload
}

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

export interface ITaskJSON {
    taskTitle: string,
    taskDescription: string,
    reward: number,
    status: UserTaskStatus,
    taskImageKey: string,
    taskId: number,
    taskType: TaskType,
    taskPromptTitle: string,
    taskPromptSubtitle: string
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
export interface TaskList {
    Tasks: ITask[],
    Loaded: boolean
}
// #endregion

// #region Third-party file upload
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
    Extension: string,
    IsPrivate: boolean
}

export interface UploadHandshakeConfirmation {
    RequestId: string,
    Status: RequestStatus
}
// #endregion

// #region Admin
export interface UserActivityLog {
    FullName: string,
    TaskName: string,
    Reward: number,
    ActivityDateTime: Date
}

export interface UserActivityLogJSON {
    fullName: string,
    task: string,
    reward: number,
    created: Date
}

export interface AdminTaskSubmission {
    SubmissionId: number,
    TaskId: number,
    UserId: string,
    UserFullName: string
}
// #endregion

// #region Auth
export interface LoginJSON {
    email: string,
    password: string
}

export interface RegisterJSON extends LoginJSON {
    FirstName: string,
    LastName: string,
    Birthday: string,
    PhoneNumber: number,
    ConfirmPassword: string
}
export interface UserCookieJSON {
    userId: string,
    email: string,
    roles: string[]
}
// #endregion

// #region UserPoints
export interface IUserPoints {
    TotalPoints: number,
    Entries: number,
    IsError: boolean,
    Loaded: boolean
}
export interface UserPointsJSON {
    userId: string,
    points: number,
    previousPoints: number,
    entries: number,
    previousEntries: number,
    updated: Date
}

export interface UserPoints {
    UserId: string,
    Points: number,
    PreviousPoints: number,
    Entries: number,
    PreviousEntries: number,
    Updated: Date
}

export interface UserResult {
    Name: string,
    Points: number,
    Entries: number
}

export interface UserResultJSON {
    name: string,
    points: number,
    entries: number
}

// #endregion

// #region Quizzes
export interface QuestionOptions {
    [key: string]: string
}

export interface TaskQuestion {
    questionId: number,
    prompt: string,
    promptImageKey: string,
    options: QuestionOptions | undefined,
    multi: boolean
}

export interface UserAnswer {
    questionId: number,
    answer: string[]
}

export interface TaskQuizAnswers {
    taskId: number,
    answers: UserAnswer[]
}

// #endregion

export enum AppMode {
    Dashboard,
    Admin,
    Login,
    Register
}

export default '.'