using seaway.API.Configurations;
using seaway.API.Models;
using System.Data.SqlClient;
using System.Data;

namespace seaway.API.Manager
{
    public class OfferManager
    {
        private readonly ILogger<LogHandler> _logger;
        private readonly IConfiguration _configuration;
        string _conString;

        public OfferManager(ILogger<LogHandler> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _conString = _configuration.GetConnectionString("DefaultConnection");
        }

        public Offer GetOfferById(int offerId)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }

        public bool ChangeActiveStatus(bool status, int id)
        {
            try
            {
                var query = "UPDATE Offers SET IsActive = @status WHERE OfferId = @Id";

                using (SqlConnection con = new SqlConnection(this._conString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        con.Open();

                        cmd.Parameters.AddWithValue("@Id", id);
                        cmd.Parameters.AddWithValue("@status", status);

                        cmd.ExecuteNonQuery();

                        _logger.LogTrace("Sucessfully Changed Offer Status of Id --> " + id + "From Database");

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
