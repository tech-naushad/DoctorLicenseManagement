using DoctorLicenseManagement.Domain.Enums;
using System.Text.Json.Serialization;

namespace DoctorLicenseManagement.Application.Commands
{
    public class DoctorCommand
    {
        [JsonPropertyName("fullName")]
        public string FullName { get; set; } = string.Empty;

        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("specialization")]
        public string Specialization { get; set; } = string.Empty;

        [JsonPropertyName("licenseNumber")]
        public string LicenseNumber { get; set; } = string.Empty;

        [JsonPropertyName("licenseExpiryDate")]
        public DateTime LicenseExpiryDate { get; set; }

        [JsonPropertyName("licenseStatus")]
        public LicenseStatus LicenseStatus { get; set; }
    }
}
