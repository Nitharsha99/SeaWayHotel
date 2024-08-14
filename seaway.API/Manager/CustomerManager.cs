using seaway.API.Configurations;
using seaway.API.Models;
using System.Data;
using System.Data.SqlClient;

namespace seaway.API.Manager
{
    public class CustomerManager
    {
        private readonly ILogger<LogHandler> _logger;
        private readonly IConfiguration _configuration;
        string _conString;
        public CustomerManager(ILogger<LogHandler> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _conString = _configuration.GetConnectionString("DefaultConnection");
        }


        public async void PostCustomer(Customer customer)
        {
            try
            { 
                using(SqlConnection con = new SqlConnection(this._conString))
                {
                    using(SqlCommand cmd = new SqlCommand("NewCustomer", con))
                    {
                        await con.OpenAsync();
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@customerName", customer.Name);
                        cmd.Parameters.AddWithValue("@email", customer.Email);
                        cmd.Parameters.AddWithValue("@contactNo", customer.ContactNo);
                        cmd.Parameters.AddWithValue("@nicNo", customer?.NIC);
                        cmd.Parameters.AddWithValue("@passportNo", customer?.PassportNo);

                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                    _logger.LogTrace("SuccessFully inserted the new customer");
            }
            catch(Exception ex)
            {
                _logger.LogWarning(" Warning -- " + ex.Message);
                throw;
            }
        }
    }
}
