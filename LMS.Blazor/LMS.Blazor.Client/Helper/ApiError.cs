using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace LMS.Blazor.Client.Helper
{
    public class ApiError
    {
        [JsonPropertyName("title")]
        public string? Title { get; set; }
        [JsonPropertyName("detail")]
        public string? Detail { get; set; }

        public override string ToString()
        {
            return $"Title: {Title}, Detail: {Detail}";
        }
    }
}
