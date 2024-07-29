using seaway.API.Configurations;
using seaway.API.Models;
using System;
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

        public bool NewAdmin(Admin admin)
        {
            try
            {
                if (admin.Password == null) return false;

                var hashPassword = _passwordHelper.HashingPassword(admin.Password);
                byte[]? picPathBytes = null;

                if (admin.ProfilePicPath != null)
                {
                   picPathBytes = Encoding.UTF8.GetBytes(admin.ProfilePicPath);
                }

                using (SqlConnection con = new SqlConnection(this._conString))
                {
                    using (SqlCommand cmd = new SqlCommand("NewAdmin", con))
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@username", admin.Username);
                        cmd.Parameters.AddWithValue("@password", hashPassword);
                        cmd.Parameters.AddWithValue("@isAdmin", admin.IsAdmin);
                        cmd.Parameters.AddWithValue("@profilePic", picPathBytes);
                        cmd.Parameters.AddWithValue("@created", admin.CreatedBy);

                        cmd.ExecuteNonQuery();
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

        public List<Admin> GetAllAdmins()
        {
            try
            {
                List<Admin> adminList = new List<Admin>();

                using (SqlConnection _con = new SqlConnection(this._conString))
                {
                    SqlCommand command = _con.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "AllAdmin";

                    _con.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string? picFile = null;
                            if(reader["ProfilePic"] != DBNull.Value)
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
                                ProfilePicPath = picFile,
                                Created = Convert.ToDateTime(reader["Created"]),
                                CreatedBy = reader["CreatedBy"].ToString(),
                                Updated = Convert.ToDateTime(reader["Updated"]),
                                UpdatedBy = reader["UpdatedBy"].ToString()
                            };

                            adminList.Add(admin);

                        }
                    }

                    _con.Close();

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

        public bool IsUsernameExist(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return false;
            }
            else
            {
                var result = GetAllAdmins()
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


    }
}
