using seaway.API.Configurations;
using seaway.API.Models;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace seaway.API.Manager
{
    public class AdminManager
    {
        private readonly ILogger<LogHandler> _logger;
        private readonly IConfiguration _configuration;
        private readonly PasswordHelper _passwordHelper;
        string _conString;


        public AdminManager(ILogger<LogHandler> logger, IConfiguration configuration, PasswordHelper passwordHelper)
        {
            _logger = logger;
            _configuration = configuration;
            _conString = _configuration.GetConnectionString("DefaultConnection");
            _passwordHelper = passwordHelper;
        }

        public async Task<bool> NewAdmin(Admin admin)
        {
            try
            {
                if (admin.Password == null) return false;

                var hashPassword = _passwordHelper.HashingPassword(admin.Password);
                byte[]? picPathBytes = null;

                if (admin.PicPath != null)
                {
                   picPathBytes = Encoding.UTF8.GetBytes(admin.PicPath);
                }

                using (SqlConnection con = new SqlConnection(this._conString))
                {
                    using (SqlCommand cmd = new SqlCommand("NewAdmin", con))
                    {
                        await con.OpenAsync();
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@username", admin.Username);
                        cmd.Parameters.AddWithValue("@password", hashPassword);
                        cmd.Parameters.AddWithValue("@isAdmin", admin.IsAdmin);
                        cmd.Parameters.AddWithValue("@profilePic", picPathBytes);
                        cmd.Parameters.AddWithValue("@picName", admin.PicName);
                        cmd.Parameters.AddWithValue("@publicId", admin.PublicId);
                        cmd.Parameters.AddWithValue("@created", admin.CreatedBy);

                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                _logger.LogTrace(LogMessages.NewRecordCreated);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(" Warning -- " + ex.Message);
                return false;
            }
        }

        public async Task<List<Admin>> GetAllAdmins()
        {
            try
            {
                List<Admin> adminList = new List<Admin>();

                using (SqlConnection _con = new SqlConnection(this._conString))
                {
                    SqlCommand command = _con.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "AllAdmin";

                    await _con.OpenAsync();

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            string? picFile = null;
                            if (reader["ProfilePic"] != DBNull.Value)
                            {
                                byte[] filePathInByte = (byte[])reader["ProfilePic"];
                                picFile = Convert.ToBase64String(filePathInByte);
                            }

                            Admin admin = new Admin
                            {
                                AdminId = (int)reader["AdminId"],
                                Username = reader["Username"].ToString(),
                                Password = reader["Password"].ToString(),
                                IsAdmin = (bool)reader["IsAdmin"],
                                PicPath = picFile,
                                Created = Convert.ToDateTime(reader["Created"]),
                                CreatedBy = reader["CreatedBy"].ToString(),
                                Updated = Convert.ToDateTime(reader["Updated"]),
                                UpdatedBy = reader["UpdatedBy"].ToString()
                            };

                            adminList.Add(admin);

                        }
                    }
                    await _con.CloseAsync();

                    _logger.LogTrace(LogMessages.AllAdminRetrieve);

                    return adminList;
                }
            }
            catch(Exception ex)
            {
                _logger.LogWarning(" Warning -- " + ex.Message);
                throw;
            }
        }

        public async Task<Admin> GetAdminById(int id)
        {
            try
            {
                List<Admin> adminList = await GetAllAdmins();

                var admin = adminList.FirstOrDefault(a => a.AdminId == id) ?? new Admin();

                return admin;

            }
            catch(Exception ex)
            {
                _logger.LogWarning(" Warning -- " + ex.Message);
                throw;
            }
        }

        public async void UpdateAdmin(Admin admin)
        {
            try
            {
                byte[]? picPathBytes = null;

                if (admin.PicPath != null)
                {
                    picPathBytes = Encoding.UTF8.GetBytes(admin.PicPath);
                }

                using (SqlConnection con = new SqlConnection(this._conString))
                {
                    using (SqlCommand cmd = new SqlCommand("UpdateAdmin", con))
                    {
                        await con.OpenAsync();
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@adminId", admin.AdminId);
                        cmd.Parameters.AddWithValue("@username", admin.Username);
                        cmd.Parameters.AddWithValue("@profilePic", picPathBytes);
                        cmd.Parameters.AddWithValue("@isAdmin", admin.IsAdmin);
                        cmd.Parameters.AddWithValue("@updatedBy", admin.UpdatedBy);


                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                _logger.LogTrace(LogMessages.RecordUpdated);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Warning at Update the " + admin.Username + " : " + ex.Message);
                throw;
            }
        }

        public async Task<bool> IsUsernameExist(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return false;
            }
            else
            {
                var result = (await GetAllAdmins())
                    .Where(a => a.Username.Trim().ToLower() == username.Trim().ToLower())
                    .ToList();

                if(result.Count() > 0)
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

        public async Task<bool> DeleteAdmin(int id)
        {
            try
            {
                var query = "DELETE FROM Admin WHERE AdminId = @adminId";

                using (SqlConnection con = new SqlConnection(this._conString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        await con.OpenAsync();

                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@adminId", id);

                        await cmd.ExecuteNonQueryAsync();

                        _logger.LogTrace("Sucessfully Deleted Admin of Id --> " + id + "From Database");

                        return true;

                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(" Warning -- " + ex.Message);
                return false;
            }
        }


    }
}
