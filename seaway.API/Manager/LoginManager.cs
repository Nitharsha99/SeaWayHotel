using System.Data.SqlClient;
using seaway.API.Models;
using seaway.API.Configurations;
using seaway.API.Models.ViewModels;
using System.Data;

namespace seaway.API.Manager
{
    public class LoginManager
    { 
        private readonly ILogger<LogHandler> _logger;
        private readonly IConfiguration _configuration;
        private readonly PasswordHelper _passwordHelper;
        private readonly AdminManager _adminManager;
        string _conString;

        public LoginManager(ILogger<LogHandler> logger, IConfiguration configuration, PasswordHelper passwordHelper, AdminManager adminManager) { 
            _logger = logger;
            _configuration = configuration;
            _passwordHelper = passwordHelper;
            _adminManager = adminManager;
            _conString = _configuration.GetConnectionString("DefaultConnection");
        }

        public bool CheckUserValid(LoginModel login)
        {
            if (string.IsNullOrEmpty(login.Password))
            {
                return false;
            }
            else
            {
                 Admin admin = _adminManager.GetAllAdmins()
                                 .FirstOrDefault(a => a.Username.Trim().ToLower() == login.Username.Trim().ToLower() 
                                 && a.IsAdmin == login.IsAdmin) ?? new Admin();
                
                bool validUser = false;

                if(admin.Password != null)
                {
                   validUser = _passwordHelper.Verification(admin.Password, login.Password);

                    if(validUser)
                    {
                        AddLoginTime(admin);
                    }
                }

                return validUser;
            }
        }

        public void AddLoginTime(Admin login)
        {
            using (SqlConnection con = new SqlConnection(this._conString))
            {
                using (SqlCommand cmd = new SqlCommand("LoginLogoutTime", con))         
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@username", login.Username);
                    cmd.Parameters.AddWithValue("@adminId", login.AdminId);

                    cmd.ExecuteNonQuery();
                }
            }

            _logger.LogTrace(LogMessages.NewRecordCreated);
        }


    }

}
