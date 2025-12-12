import type { ITask, LoginJSON } from "../models/ViewModels";
import { APICall, type APIResponse } from "./APIService";

export async function AttemptAuth(loginModel: LoginJSON, register = false): Promise<APIResponse> {
    var res = await APICall<APIResponse, LoginJSON>("Login", "POST", loginModel, register)

    return res;
}