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
                _logger.LogWarning(LogMessages.Warning + ex.Message);
                return false;
            }
        }


    }
}
