using Dapper;
using DoctorLicenseManagement.Domain.Entities;
using DoctorLicenseManagement.Domain.Enums;
using DoctorLicenseManagement.Infrastructure.Data;
using Microsoft.Extensions.Logging;
using System.Data;

namespace DoctorLicenseManagement.Infrastructure.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly IDbConnectionFactory _factory;
        private readonly ILogger<DoctorRepository> _logger;

        public DoctorRepository(IDbConnectionFactory factory, ILogger<DoctorRepository> logger)
        {
            _factory = factory;
            _logger = logger;
        }

        public async Task<(IEnumerable<Doctor> Doctors, int TotalCount)> GetAllAsync(string? search, LicenseStatus? licenseStatus, 
            int page, int pageSize)
        {
            try
            {
                // Call your stored procedure with paging params
                var parameters = new DynamicParameters();
                parameters.Add("@Search", search);
                parameters.Add("@Status", licenseStatus);
                parameters.Add("@Page", page);
                parameters.Add("@PageSize", pageSize);

                using var connection = _factory.CreateConnection();

                using var multi = await connection.QueryMultipleAsync(
                    "sp_GetDoctors",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                var doctors = await multi.ReadAsync<Doctor>();
                var totalCount = await multi.ReadFirstAsync<int>();
                return (doctors, totalCount);
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
                using var con = _factory.CreateConnection();

                return await con.QueryFirstOrDefaultAsync(
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
                using var con = _factory.CreateConnection();

                var parameters = new DynamicParameters();
                parameters.Add("@FullName", doctor.FullName);
                parameters.Add("@Email", doctor.Email);
                parameters.Add("@Specialization", doctor.Specialization);
                parameters.Add("@LicenseNumber", doctor.LicenseNumber);
                parameters.Add("@LicenseExpiryDate", doctor.LicenseExpiryDate);
                parameters.Add("@LicenseStatus", (int)doctor.LicenseStatus);
                

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
                using var con = _factory.CreateConnection();

                var parameters = new DynamicParameters();
                parameters.Add("@Id", doctor.Id);
                parameters.Add("@FullName", doctor.FullName);
                parameters.Add("@Email", doctor.Email);
                parameters.Add("@Specialization", doctor.Specialization);
                parameters.Add("@LicenseNumber", doctor.LicenseNumber);
                parameters.Add("@LicenseExpiryDate", doctor.LicenseExpiryDate);
                parameters.Add("@Status", (int)doctor.LicenseStatus);

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

        public async Task<(bool Success, string Message)> DeleteAsync(int id)
        {
            try
            {
                using var con = _factory.CreateConnection();

                var parameters = new DynamicParameters();
                parameters.Add("@Id", id);
                parameters.Add("@Message", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

                var rows = await con.ExecuteAsync(
                    "sp_DeleteDoctor",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                var message = parameters.Get<string>("@Message");

                return (rows > 0, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting doctor with Id {Id}", id);

                return (false, "Error occurred while deleting doctor");
            }
        }
    }
}