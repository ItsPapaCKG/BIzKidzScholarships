import type { ITask, LoginJSON } from "../models/ViewModels";
import { APICall, type APIResponse } from "./APIService";

export async function AttemptAuth(loginModel: LoginJSON, register = false): Promise<APIResponse> {
    let endpoint = register ? "Register" : "Login";

    var res = await APICall<any, LoginJSON>(endpoint, "POST", loginModel, true)

    return res;
}