
import { config as appConfig } from './ConfigService';

// Any component can request data

// The response is either:
// response.Success or response.Error

// then from there

// hooks to determine whether to redirect to login
// set the error state
// or some other custom action

export enum ResponseCode {
    ServerError = -1,
    Success,
    Unauthorized,
    Forbidden,
    NotFound
}

export type APIResponse<T = unknown> = |
{
    success: true,
    data: T
}
    |
{
    success: false,
    error: APIErrorMessage
}

export type APIErrorMessage = |
{
    status: ResponseCode.Forbidden
    message: string
} |
{
    status: ResponseCode.ServerError
    message: string
} |
{
    status: ResponseCode
    message?: string
}


export function ResponseError(code: number, text?: string): APIErrorMessage {

    if (code == 401) {
        return {
            status: ResponseCode.Unauthorized
        } as APIErrorMessage
    }

    if (code == 403) {
        return {
            status: ResponseCode.Forbidden,
            message: text ?? "You do not have the permissions to perform this action."
        } as APIErrorMessage
    }

    if (code == 404) {
        return {
            status: ResponseCode.NotFound
        } as APIErrorMessage
    }

    return {
        status: ResponseCode.ServerError,
        message: text ?? "An internal server error has occurred."
    } as APIErrorMessage

}

export async function APICall<Output = unknown, Input = unknown>(urlPath: string, method: string, data?: Input, auth = false): Promise<APIResponse<Output>> {
    var config: RequestInit = {
        method: method,
        credentials: "include"
    }

    if (method == "POST" && data != undefined) {
        console.log("Sending JSON: "+JSON.stringify(data));
        config.body = JSON.stringify(data)
        config.headers = {
            "Content-Type": "application/json"
        }
    }

    let prefix = auth ? "/auth/" : "/api/"

    var res = await fetch(`${appConfig.baseAPIURL}:${appConfig.apiPort}${prefix}${urlPath}`, config);

    if (!res.ok) {
        var code = res.status;
        var text = await res.text();

        return {
            success: false,
            error: ResponseError(code, text)
        } as APIResponse<Output>
    }

    if (res.headers.get("Content-Length") === "0") {
        return {
            success: true,
            data: {}
        } as APIResponse<Output>
    }

    var json = await res.json() as Output;

    return {
        success: true,
        data: json
    } as APIResponse<Output>
}