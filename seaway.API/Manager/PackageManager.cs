using seaway.API.Configurations;
using seaway.API.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using seaway.API.Models.Enum;

namespace seaway.API.Manager
{
    public class PackageManager
    {
        private readonly ILogger<LogHandler> _logger;
        private readonly IConfiguration _configuration;
        string _conString;

        public PackageManager(ILogger<LogHandler> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _conString = _configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<Package>> GetAllPackages()
        {
            try
            {
                List<Package> packages = new List<Package>();
                var query = "SELECT * FROM Packages";

                using (SqlConnection _con = new SqlConnection(this._conString))
                {
                    SqlCommand command = _con.CreateCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = query; 

                    await _con.OpenAsync();

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Package package = new Package
                            {
                                Id = (int)reader["PackageId"],
                                Name = reader["PackageName"].ToString(),
                                Description = reader["Description"].ToString(),
                                DurationType = (PackageDurationType)reader["PackageDurationType"],
                                Price = reader["Price"] == DBNull.Value ? 0.0 : Convert.ToDouble(reader["Price"]),
                                UserType = (UserType)reader["UserType"],
                                IsActive = Convert.ToBoolean(reader["IsActive"])
                            };

                            packages.Add(package);
                        }
                    }

                    await _con.CloseAsync();

                    _logger.LogTrace("SuccessFully All Packages Data retrieved");

                    return packages;
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(" Warning -- " + ex.Message);
                throw;
            }
        }
    }
}
