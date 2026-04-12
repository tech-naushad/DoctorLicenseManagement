using DoctorLicenseManagement.Application.Queries;
using System.Text.Json.Serialization;


namespace DoctorLicenseManagement.Application.Common
{
    public class ApiResponse
    {
        [JsonPropertyName("error")]
        public string Error { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("success")]
        public bool Success { get; set; }
    }
    public class ApiPageResponse: PageQueryResponse
    {
        [JsonPropertyName("error")]
        public string Error { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("success")]
        public bool Success { get; set; }
    }
    
}
