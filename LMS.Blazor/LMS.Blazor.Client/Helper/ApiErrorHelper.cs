using System.Text.Json;

namespace LMS.Blazor.Client.Helper
{
    public static class ApiErrorHelper
    {
        public static async Task<string?> ReadErrorCodeAsync(
            HttpResponseMessage response)
        {
            try
            {
                using var document = JsonDocument.Parse(
                    await response.Content.ReadAsStringAsync());

                if (document.RootElement.TryGetProperty(
                        "errorCode", out var codeElement)
                    && codeElement.ValueKind == JsonValueKind.String)
                {
                    return codeElement.GetString();
                }
            }
            catch (JsonException)
            {
                return null;
            }

            return null;
        }
    }
}