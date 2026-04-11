using DoctorLicenseManagement.Domain.Entities;
using DoctorLicenseManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorLicenseManagement.Infrastructure.Repositories 
{ 
    public interface IDoctorRepository
    {
        Task<(IEnumerable<Doctor> Doctors, int TotalCount)> GetAllAsync
            (string? search, LicenseStatus? licenseStatus,
            int page, int pageSize);
        Task<Doctor?> GetByIdAsync(int id);
        Task<int> CreateAsync(Doctor doctor);
        Task<bool> UpdateAsync(Doctor doctor);
        Task<bool> DeleteAsync(int id);
    }
}
