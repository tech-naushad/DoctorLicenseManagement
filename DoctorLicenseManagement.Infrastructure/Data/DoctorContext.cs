using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorLicenseManagement.Infrastructure.Data
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
    public class SqlConnectionFactory : IDbConnectionFactory
    {
        private readonly IConfiguration _config;

        public SqlConnectionFactory(IConfiguration config)
        {
            _config = config;
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(
                _config.GetConnectionString("DoctorLicenseConnection")
            );
        }
    }
    //public class DoctorContext
    //{
    //    private readonly IConfiguration _config;

    //    public DoctorContext(IConfiguration config)
    //    {
    //        _config = config;
    //    }

    //    public IDbConnection CreateConnection()
    //    {
    //        return new SqlConnection(_config.GetConnectionString("DoctorLicenseConnection")); 
    //    }
         
    //}
}
