using Dapper;
using DoctorLicenseManagement.Domain.Entities;
using DoctorLicenseManagement.Infrastructure.Data;
using Microsoft.Extensions.Logging;
using System.Data;

namespace DoctorLicenseManagement.Infrastructure.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly DoctorContext _context;
        private readonly ILogger<DoctorRepository> _logger;

        public DoctorRepository(DoctorContext context, ILogger<DoctorRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Doctor>> GetAllAsync()
        {
            try
            {
                using var con = _context.CreateConnection();

                return await con.QueryAsync<Doctor>(
                    "sp_GetDoctors",
                    commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all doctors");
                throw;
            }
        }

        public async Task<Doctor?> GetByIdAsync(int id)
        {
            try
            {
                using var con = _context.CreateConnection();

                return await con.QueryFirstOrDefaultAsync<Doctor>(
                    "sp_GetDoctorById",
                    new { Id = id },
                    commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching doctor with Id {Id}", id);
                throw;
            }
        }

        public async Task<int> CreateAsync(Doctor doctor)
        {
            try
            {
                using var con = _context.CreateConnection();

                var parameters = new DynamicParameters();
                parameters.Add("@FullName", doctor.FullName);
                parameters.Add("@Email", doctor.Email);
                parameters.Add("@Specialization", doctor.Specialization);
                parameters.Add("@LicenseNumber", doctor.LicenseNumber);
                parameters.Add("@LicenseExpiryDate", doctor.LicenseExpiryDate);
                parameters.Add("@Status", (int)doctor.Status);
                parameters.Add("@CreatedDate", doctor.CreatedDate);

                return await con.ExecuteScalarAsync<int>(
                    "sp_CreateDoctor",
                    parameters,
                    commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating doctor");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(Doctor doctor)
        {
            try
            {
                using var con = _context.CreateConnection();

                var parameters = new DynamicParameters();
                parameters.Add("@Id", doctor.Id);
                parameters.Add("@FullName", doctor.FullName);
                parameters.Add("@Email", doctor.Email);
                parameters.Add("@Specialization", doctor.Specialization);
                parameters.Add("@LicenseNumber", doctor.LicenseNumber);
                parameters.Add("@LicenseExpiryDate", doctor.LicenseExpiryDate);
                parameters.Add("@Status", (int)doctor.Status);

                var rows = await con.ExecuteAsync(
                    "sp_UpdateDoctor",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                return rows > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating doctor with Id {Id}", doctor.Id);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                using var con = _context.CreateConnection();

                var rows = await con.ExecuteAsync(
                    "sp_DeleteDoctor",
                    new { Id = id },
                    commandType: CommandType.StoredProcedure);

                return rows > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting doctor with Id {Id}", id);
                throw;
            }
        }
    }
}