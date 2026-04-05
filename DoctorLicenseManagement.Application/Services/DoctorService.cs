using DoctorLicenseManagement.Application.DTOs;
using DoctorLicenseManagement.Application.Interfaces;
using DoctorLicenseManagement.Domain.Entities;
using DoctorLicenseManagement.Domain.Enums;
using DoctorLicenseManagement.Infrastructure.Repositories;

namespace DoctorLicenseManagement.Application.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _repo;

        public DoctorService(IDoctorRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<DoctorDto>> GetAllAsync()
        {
            var data = await _repo.GetAllAsync();
            return data.Select(MapToDto);
        }

        public async Task<DoctorDto?> GetByIdAsync(int id)
        {
            var doctor = await _repo.GetByIdAsync(id);
            return doctor == null ? null : MapToDto(doctor);
        }

        public async Task<int> CreateAsync(DoctorDto dto)
        {
            Validate(dto);

            var entity = MapToEntity(dto);
            entity.CreatedDate = DateTime.UtcNow;
            entity.Status = LicenseStatus.Active;

            return await _repo.CreateAsync(entity);
        }

        public async Task<bool> UpdateAsync(DoctorDto dto)
        {
            Validate(dto);
            return await _repo.UpdateAsync(MapToEntity(dto));
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }

        private static void Validate(DoctorDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.FullName))
                throw new ArgumentException("Full Name is required");

            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new ArgumentException("Email is required");

            if (dto.LicenseExpiryDate < DateTime.UtcNow.Date)
                dto.Status = LicenseStatus.Expired;
        }

        private static DoctorDto MapToDto(Doctor d) => new()
        {
            Id = d.Id,
            FullName = d.FullName,
            Email = d.Email,
            Specialization = d.Specialization,
            LicenseNumber = d.LicenseNumber,
            LicenseExpiryDate = d.LicenseExpiryDate,
            Status = d.Status
        };
        private static Doctor MapToEntity(DoctorDto d) => new()
        {
            Id = d.Id,
            FullName = d.FullName,
            Email = d.Email,
            Specialization = d.Specialization,
            LicenseNumber = d.LicenseNumber,
            LicenseExpiryDate = d.LicenseExpiryDate,
            Status = d.Status
        };
    }
}
