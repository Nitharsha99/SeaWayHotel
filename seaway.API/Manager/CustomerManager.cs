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


        public async Task<int> PostCustomer(Customer customer)
        {
            try
            { 
                int customerId = 0;
                using(SqlConnection con = new SqlConnection(this._conString))
                {
                    using(SqlCommand cmd = new SqlCommand("NewCustomer", con))
                    {
                        await con.OpenAsync();
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@customerName", customer.Name);
                        cmd.Parameters.AddWithValue("@email", customer.Email_add);
                        cmd.Parameters.AddWithValue("@contactNo", customer.ContactNo);
                        cmd.Parameters.AddWithValue("@nicNo", customer?.NIC);
                        cmd.Parameters.AddWithValue("@passportNo", customer?.PassportNo);

                        customerId = (int?)(await cmd.ExecuteScalarAsync()) ?? 0;
                    }
                }

                    _logger.LogTrace("SuccessFully inserted the new customer");
                return customerId;
            }
            catch(Exception ex)
            {
                _logger.LogWarning(" Warning -- " + ex.Message);
                throw;
            }
        }

        public async Task<Customer> GetByUniqueId(string uniqueId)
        {
            try
            {
                if (string.IsNullOrEmpty(uniqueId))
                {
                    return new Customer();
                }
                else
                {
                    var query = "SELECT * FROM Customer WHERE (NIC_NO = @uniqueId OR PassportNo = @uniqueId)";
                    Customer customer = new Customer();

                    using (SqlConnection con = new SqlConnection(this._conString))
                    {
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            await con.OpenAsync();

                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@uniqueId", uniqueId);

                            using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                            {
                                while (await reader.ReadAsync())
                                {

                                    customer.Id = (int)reader["CustomerId"];
                                    customer.Name = reader["Name"].ToString();
                                    customer.Email_add = reader["Email"].ToString();
                                    customer.ContactNo = reader["ContactNo"].ToString();
                                    customer.NIC = reader["NIC_No"].ToString();
                                    customer.PassportNo = reader["PassportNo"].ToString();     

                                }
                            }

                            _logger.LogTrace(LogMessages.GetExistData);

                            return customer;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                _logger.LogWarning(" Warning -- " + ex.Message);
                throw;
            }
        }
    }
}
