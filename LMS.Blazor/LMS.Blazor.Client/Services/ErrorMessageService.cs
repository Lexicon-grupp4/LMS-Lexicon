using System.Net;

namespace LMS.Blazor.Client.Services;

public static class ErrorMessageService
{
    public static string GetFriendlyErrorMessage(
        HttpStatusCode? statusCode,
        string? errorCode = null)
    {
        return errorCode switch
        {
            "EMAILALREADYTAKEN" =>
                "This email address is already taken.",

            "CANNOTDELETEOWN_ACCOUNT" =>
                "You cannot delete your own account. Please contact an administrator.",

            "RESOURCENOTFOUND" =>
                "The requested resource was not found.",

            "TOKEN_INVALID" =>
                "The token is invalid or has expired. Please try again.",

            "INVALID_REQUEST" =>
                "The request is invalid. Please check your input.",

            "INTERNAL_ERROR" =>
                "An unexpected server error occurred. Please try again.",

            _ => GetMessageFromStatusCode(statusCode)
        };
    }

    private static string GetMessageFromStatusCode(
        HttpStatusCode? statusCode)
    {
        return statusCode switch
        {
            HttpStatusCode.Unauthorized =>
                "Your session has expired. Please log in again.",

            HttpStatusCode.Forbidden =>
                "You are not authorized to perform this action.",

            HttpStatusCode.BadRequest =>
                "The request is invalid. Please check your input.",

            HttpStatusCode.NotFound =>
                "The requested resource was not found.",

            HttpStatusCode.Conflict =>
                "The request conflicts with the current state.",

            _ =>
                "Unable to complete the request. Please try again."
        };
    }
}