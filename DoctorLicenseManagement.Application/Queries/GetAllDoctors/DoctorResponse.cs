using DoctorLicenseManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DoctorLicenseManagement.Application.Queries.GetAllDoctors
{
    public class DoctorResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("full_name")]
        public string FullName { get; set; } = string.Empty;

        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("specialization")]
        public string Specialization { get; set; } = string.Empty;

        [JsonPropertyName("license_number")]
        public string LicenseNumber { get; set; } = string.Empty;

        [JsonPropertyName("license_expiry_date")]
        public DateTime LicenseExpiryDate { get; set; }

        [JsonPropertyName("license_status")]
        public LicenseStatus LicenseStatus { get; set; }
    }
}
