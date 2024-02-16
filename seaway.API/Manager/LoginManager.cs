using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using seaway.API.Controllers;
using seaway.API.Models;
using seaway.API.Configurations;

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

        public List<Admin> GetAdmin()
        {
            try
            {
                List<Admin> resValue = new List<Admin>();

                using (SqlConnection _con = new SqlConnection(this._conString))
                {
                    SqlCommand command = _con.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "AllAdmin";
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();

                    _con.Open();
                    adapter.Fill(dt);
                    _con.Close();

                    foreach (DataRow row in dt.Rows)
                    {
                        resValue.Add(new Admin
                        {
                            Username = row["Username"].ToString(),
                            Password = row["Password"].ToString()
                        });
                    }
                }

                _logger.LogTrace("SuccessFully Admin Data retrieved");

                return resValue;
            }
            catch(Exception ex)
            {
                _logger.LogWarning("Warning -- " + ex.Message);
                throw;
            }

        }
    }
}
