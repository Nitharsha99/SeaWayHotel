using seaway.API.Configurations;
using seaway.API.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using seaway.API.Models.Enum;
using System.Diagnostics;

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

        public async Task<int> NewPackage(Package package)
        {
            try
            {
                int packageId = 0;
                using (SqlConnection _con = new SqlConnection(this._conString))
                {
                    using (SqlCommand command = new SqlCommand("InsertPackage", _con))
                    {
                        await _con.OpenAsync();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();

                        command.Parameters.AddWithValue("@name", package.Name);
                        command.Parameters.AddWithValue("@description", package.Description);
                        command.Parameters.AddWithValue("@durationType", package.DurationType);
                        command.Parameters.AddWithValue("@price", package.Price);
                        command.Parameters.AddWithValue("@userType", package.UserType);
                        command.Parameters.AddWithValue("@createdBy", package.CreatedBy);

                        packageId = (int?)(await command.ExecuteScalarAsync()) ?? 0;


                    }
                }

                _logger.LogTrace("Get Package Id --> " + packageId);

                return packageId;
            }
            catch(Exception ex)
            {
                _logger.LogWarning("Warning at Post Package : " + ex.Message);
                throw;
            }
        }

        public async Task<bool> IsNameExist(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }
            else
            {
                var result = (await GetAllPackages())
                    .Where(a => a.Name.Trim().ToLower() == name.Trim().ToLower())
                    .ToList();

                if (result.Count() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsNameChange(string inputName, string oldName)
        {
            if (string.IsNullOrEmpty(inputName) || string.IsNullOrEmpty(oldName)) { return false; }
            else
            {
                bool isNameChange = inputName.Trim().ToLower() != oldName.Trim().ToLower();
                return isNameChange;
            }
        }
    }
}
