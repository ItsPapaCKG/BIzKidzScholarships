import { type UserActivityLogJSON, type UserActivityLog, type UserResult, type UserResultJSON, type TaskList, type ITask, type SubmissionsSearchResults, type GetTasksResponse, type GetSubmissionResponse } from "../models/ViewModels";
import { APICall, type APIResponse } from "./APIService";

export async function GetUserActivities(): Promise<UserActivityLog[]> {
    // TODO send a request to get the UserActivityView

    let res = await APICall<UserActivityLogJSON[]>("admin/activities", "GET");

    if (res.success) {
        let result: UserActivityLog[] = []

        res.data!.forEach((v) => {
            result.push({ FullName: v.fullName, TaskName: v.task, Reward: v.reward, ActivityDateTime: v.created } as UserActivityLog)
        });

        return result;
    }

    return [];
}

export async function GetUserResults(): Promise<UserResult[]> {
    let res = await APICall<UserResultJSON[]>("admin/getusers","GET");

    if (res.success) {
        let result: UserResult[] = [];

        res.data!.forEach((k) => {
            result.push({ ChildFullName: k.childFullName, Email: k.email, UserId: k.userId, Name: k.name, Points: k.points, Entries: k.entries, Created: new Date(k.created), Updated: new Date(k.updated), UserType: k.userType } as UserResult)
        });

        return result;
    }

    return [];
}

export async function GetAllTasks(): Promise<GetTasksResponse> {
    let res = await APICall<GetTasksResponse>(`admin/Tasks/`, "GET");

    if (!res.success) {
        let r: GetTasksResponse = {
            Results: [],
            Error: res.error.message!
        }

        return r;
    }

    return res.data!;
}

export async function GetTaskDetails(taskId: number): Promise<ITask> {
    return {} as ITask;
}

export async function SaveTask(task: ITask): Promise<APIResponse<undefined>> {
    return {} as APIResponse<undefined>;
}

export async function GetSubmission(submissionId: string): Promise<APIResponse<GetSubmissionResponse>> {
    let res = await APICall<GetSubmissionResponse>(`admin/getsubmission/${submissionId}`, "GET");

    return res;
}

export async function GetSubmissions(taskId: number): Promise<SubmissionsSearchResults> {
    let res = await APICall<SubmissionsSearchResults>(`admin/submissions/${taskId}`, "GET");

    if (!res.success) {
        let response: SubmissionsSearchResults = {
            Results: [],
            Error: res.error.message!
        };

        return response;
    }

    for (const item of res.data!.results) {

        if (!(item.created instanceof Date)) {
            item.created = new Date(item.created);
        }
    }

    return res.data!;
}

export async function GetAllSubmissions() {
    let res = await APICall<SubmissionsSearchResults>(`admin/submissions/`, "GET");

    if (!res.success) {
        let response: SubmissionsSearchResults = {
            Results: [],
            Error: res.error.message!
        };

        return response;
    }

    for (const item of res.data!.results) {

        if (!(item.created instanceof Date)) {
            item.created = new Date(item.created);
        }
    }

    return res.data!;
}