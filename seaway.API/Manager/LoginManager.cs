using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using seaway.API.Controllers;
using seaway.API.Models;
using seaway.API.Configurations;
using System.Text;
using System.Security.Cryptography;

namespace seaway.API.Manager
{
    public class LoginManager
    { 
        private readonly ILogger<LogHandler> _logger;
        private readonly IConfiguration _configuration;
        string _conString;
        public LoginManager(ILogger<LogHandler> logger, IConfiguration configuration) { 
            _logger = logger;
            _configuration = configuration;
            _conString = _configuration.GetConnectionString("DefaultConnection");
        }

        //public List<Admin> GetAdmin()
        //{
        //    try
        //    {
        //        List<Admin> resValue = new List<Admin>();

        //        using (SqlConnection _con = new SqlConnection(this._conString))
        //        {
        //            SqlCommand command = _con.CreateCommand();
        //            command.CommandType = CommandType.StoredProcedure;
        //            command.CommandText = "AllAdmin";
        //            SqlDataAdapter adapter = new SqlDataAdapter(command);
        //            DataTable dt = new DataTable();

        //            _con.Open();
        //            adapter.Fill(dt);
        //            _con.Close();

        //            foreach (DataRow row in dt.Rows)
        //            {
        //                var encryptPassword = row["Password"].ToString() ?? "";
        //                resValue.Add(new Admin
        //                {
        //                    AdminId = (int)row["AdminId"],
        //                    Username = row["Username"].ToString(),
        //                    Password = PasswordHelper.DecryptPassword(encryptPassword)
        //                });
        //            }
        //        }

        //        _logger.LogTrace("SuccessFully Admin Data retrieved");

        //        return resValue;
        //    }
        //    catch(Exception ex)
        //    {
        //        _logger.LogWarning("Warning -- " + ex.Message);
        //        throw;
        //    }

        //}

        public Admin NewAdmin(Admin admin)
        {
            using(SqlConnection con = new SqlConnection(this._conString))
            {
                using(SqlCommand cmd = new SqlCommand("NewAdmin", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    var username = admin.Username;
                    var password = admin.Password;

                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);

                    cmd.ExecuteNonQuery();
                    _logger.LogTrace("new admin -- " + admin.Username);
                }
            }

            _logger.LogTrace("SuccessFully created new admin");

            return admin;
        }

        //public bool CheckUserValid(Admin login)
        //{
        //    if (string.IsNullOrEmpty(login.Password))
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        List<Admin> admins = GetAdmin();
        //        bool validUser = false;

        //        foreach (Admin admin in admins)
        //        {
        //            if(admin.Password != null)
        //            {
        //                if (admin.Password == login.Password)
        //                {
        //                    if (admin.Username == login.Username)
        //                    {
        //                        validUser = true;
        //                        // AddLoginTime(admin);
        //                    }
        //                }
        //            }
        //        }

        //        return validUser;
        //    }
        //}

        public void AddLoginTime(Admin login)
        {
            using (SqlConnection con = new SqlConnection(this._conString))
            {
                using (SqlCommand cmd = new SqlCommand("LoginLogoutTime", con))         
                {
                    con.Open();

                    cmd.Parameters.AddWithValue("@username", login.Username);
                    cmd.Parameters.AddWithValue("@adminId", login.AdminId);

                    cmd.ExecuteNonQuery();
                }
            }

            _logger.LogTrace("SuccessFully created new adminLogin");
        }


    }

}
