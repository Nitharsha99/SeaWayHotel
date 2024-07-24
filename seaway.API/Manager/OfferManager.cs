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

        public List<Offer> GetOffers()
        {
            try
            {
                List<Offer> offerList = new List<Offer>();

                using (SqlConnection _con = new SqlConnection(this._conString))
                {
                    SqlCommand command = _con.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "GetAllOffers";               

                    _con.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                                Offer offer = new Offer
                                {
                                    OfferId = (int)reader["OfferId"],
                                    Name = reader["OfferName"].ToString(),
                                    Description = reader["Description"].ToString(),
                                    IsRoomOffer = (bool)reader["IsRoomOffer"],
                                    IsActive = (bool)reader["IsActive"],
                                    ValidFrom = Convert.ToDateTime(reader["ValidFrom"]),
                                    ValidTo = Convert.ToDateTime(reader["ValidTo"]),
                                    Price = Convert.ToDouble(reader["Price"]),
                                    DiscountPercentage = reader["DiscountPercent"] == DBNull.Value ? 0.0 : Convert.ToDouble(reader["DiscountPercent"]),
                                    DiscountAmount = reader["DiscountPrice"] == DBNull.Value ? 0.0 : Convert.ToDouble(reader["DiscountPrice"]),
                                };

                                offerList.Add(offer);
                            

                        }
                    }

                    _con.Close();

                    _logger.LogTrace("SuccessFully All Offer Data retrieved");

                    return offerList;
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(" Warning -- " + ex.Message);
                throw;
            }
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
                    _logger.LogTrace(LogMessages.AllOfferRetrieve);
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


        public int NewOffer(Offer offer)
        {
            try
            {
                int offerId = 0;
                using (SqlConnection con = new SqlConnection(this._conString))
                {
                    using (SqlCommand cmd = new SqlCommand("InsertNewOffer", con))
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@offerName", offer.Name);
                        cmd.Parameters.AddWithValue("@description", offer.Description);
                        cmd.Parameters.AddWithValue("@isRoomOffer", offer.IsRoomOffer);
                        cmd.Parameters.AddWithValue("@validFrom", offer.ValidFrom);
                        cmd.Parameters.AddWithValue("@validTo", offer.ValidTo);
                        cmd.Parameters.AddWithValue("@price", offer.Price);
                        cmd.Parameters.AddWithValue("@discountPercentage", offer.DiscountPercentage);

                        offerId = (int)cmd.ExecuteScalar();
                    }
                }

                _logger.LogTrace(LogMessages.NewRecordCreated);

                return offerId;
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

        public void UpdateOffer(Offer offer, int offerId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(this._conString))
                {
                    using (SqlCommand cmd = new SqlCommand("UpdateOffer", con))
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@offerId", offerId);
                        cmd.Parameters.AddWithValue("@offerName", offer?.Name);
                        cmd.Parameters.AddWithValue("@description", offer?.Description);
                        cmd.Parameters.AddWithValue("@validFrom", offer?.ValidFrom);
                        cmd.Parameters.AddWithValue("@validTo", offer?.ValidTo);
                        cmd.Parameters.AddWithValue("@price", offer?.Price);
                        cmd.Parameters.AddWithValue("@discountPercent", offer?.DiscountPercentage);
                        cmd.Parameters.AddWithValue("@isActive", offer?.IsActive);
                        cmd.Parameters.AddWithValue("@isRoomOffer", offer?.IsRoomOffer);

                        cmd.ExecuteNonQuery();
                    }
                }

                _logger.LogTrace("SuccessFully updated the offer");
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Warning at Update the " + offer.Name + " : " + ex.Message);
                throw;
            }
        }

        public bool DeleteOffer(int offerId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(this._conString))
                {
                    using (SqlCommand cmd = new SqlCommand("DeleteOfferWithPics", con))
                    {
                        con.Open();

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@offerId", offerId);

                        cmd.ExecuteNonQuery();

                        _logger.LogTrace("Sucessfully Deleted Offers of Id --> " + offerId + "From Database");

                        return true;

                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogWarning(" Warning -- " + e.Message);
                return false;
            }
        }
    }
}
