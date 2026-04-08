using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorLicenseManagement.Application.Common
{
    public class ApiResponse
    {
        public string Error { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
    }
}
