using System.Net;

namespace LMS.Blazor.Client.Helper
{
    public sealed class ApiException : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public string? ErrorCode { get; }

        public ApiException(
            HttpStatusCode statusCode,
            string? errorCode)
            : base($"API request failed with status code {(int)statusCode}.")
        {
            StatusCode = statusCode;
            ErrorCode = errorCode;
        }
    }
}
