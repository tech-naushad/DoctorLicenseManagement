using DoctorLicenseManagement.Application.Commands.CreateDoctorCommand;
using DoctorLicenseManagement.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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

        [JsonPropertyName("status")]
        public LicenseStatus Status { get; set; }
    }
}
