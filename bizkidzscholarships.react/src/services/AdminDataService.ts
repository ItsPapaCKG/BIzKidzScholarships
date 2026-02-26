import { type UserActivityLogJSON, type UserActivityLog, type UserResult, type UserResultJSON, type TaskList, type ITask } from "../models/ViewModels";
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

export async function GetUserResults() {
    let res = await APICall<UserResultJSON[]>("admin/getusers","GET");

    if (res.success) {
        let result: UserResult[] = [];

        res.data!.forEach((k) => {
            result.push({ Name: k.name, Points: k.points, Entries: k.entries} as UserResult)
        });

        return result;
    }

    return [];
}

export async function GetAllTasks(): Promise<TaskList> {
    return {} as TaskList;
}

export async function GetTaskDetails(taskId: number): Promise<ITask> {
    return {} as ITask;
}

export async function SaveTask(task: ITask): Promise<APIResponse<undefined>> {
    return {} as APIResponse<undefined>;
}

export async function GetSubmission(submissionId: string): Promise<APIResponse<string>> {
    return {} as APIResponse<string>;
}