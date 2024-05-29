using seaway.API.Configurations;
using seaway.API.Models;
using System.Data.SqlClient;
using System.Data;
using seaway.API.Models.ViewModels;

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
                List<Offer> offerList = new List<Offer>();
                Offer mainoffer = new Offer();

                using (SqlConnection _con = new SqlConnection(this._conString))
                {
                    _con.Open();
                    using (var cmd = new SqlCommand("GetOfferById", _con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@OfferId", offerId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            
                            while (reader.Read())
                            {
                                var Id = (int)reader["OfferId"];
                                Offer offer = offerList.FirstOrDefault(o => o.OfferId == Id) ?? new Offer();


                                if (offer.OfferId != null)
                                {
                                    offer = new Offer
                                    {
                                        OfferId = (int)reader["OfferId"],
                                        Name = reader["Name"].ToString(),
                                        Description = reader["Description"].ToString(),
                                        Price = Convert.ToDouble(reader["Price"]),
                                        DiscountPercentage = reader["DiscountPercentage"] == DBNull.Value ? 0.0 : Convert.ToDouble(reader["DiscountPercentage"]),
                                        DiscountAmount = reader["DiscountAmount"] == DBNull.Value ? 0.0 : Convert.ToDouble(reader["DiscountAmount"]),
                                        IsRoomOffer = (bool?)reader["IsRoomOffer"],
                                        IsActive = (bool?)reader["IsActive"],
                                        ValidFrom = (DateTime?)reader["ValidFrom"],
                                        ValidTo = (DateTime?)reader["ValidTo"]

                                    };

                                    offerList.Add(offer);
                                }


                            }
                        }
                    }

                    _con.Close();
                    _logger.LogTrace("SuccessFully All Offer Data retrieved");
                    mainoffer = offerList.FirstOrDefault() ?? new Offer();
                }
                return mainoffer;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(" Warning -- " + ex.Message);
                throw;
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
