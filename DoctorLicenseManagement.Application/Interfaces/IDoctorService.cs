using DoctorLicenseManagement.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorLicenseManagement.Application.Interfaces
{
    public interface IDoctorService
    {
        Task<IEnumerable<DoctorDto>> GetAllAsync();
        Task<DoctorDto?> GetByIdAsync(int id);
        Task<int> CreateAsync(DoctorDto dto);
        Task<bool> UpdateAsync(DoctorDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
