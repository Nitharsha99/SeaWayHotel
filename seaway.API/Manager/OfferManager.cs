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

        public async Task<List<Offer>> GetOffers()
        {
            try
            {
                List<Offer> offerList = new List<Offer>();

                using (SqlConnection _con = new SqlConnection(this._conString))
                {
                    SqlCommand command = _con.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "GetAllOffers";               

                    await _con.OpenAsync();

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
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

                    await _con.CloseAsync();

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

        public async Task<Offer> GetOfferById(int offerId)
        {
     
            try
            {
                List<Offer> offerList = new List<Offer>();
                Offer mainoffer = new Offer();

                using (SqlConnection _con = new SqlConnection(this._conString))
                {
                    await _con.OpenAsync();
                    using (var cmd = new SqlCommand("GetOfferById", _con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@OfferId", offerId);

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            
                            while (await reader.ReadAsync())
                            {
                                var Id = (int)reader["OfferId"];
                                Offer offer = offerList.FirstOrDefault(o => o.OfferId == Id) ?? new Offer();


                                if (offer?.OfferId == 0)
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
                                        IsActive = (bool)reader["IsActive"],
                                        ValidFrom = (DateTime?)reader["ValidFrom"],
                                        ValidTo = (DateTime?)reader["ValidTo"],
                                        Created = (DateTime)reader["Created"],
                                        CreatedBy = reader["CreatedBy"].ToString(),
                                        Updated = (DateTime)reader["Updated"],
                                        UpdatedBy = reader["UpdatedBy"].ToString()

                                    };

                                    offerList.Add(offer);
                                }


                            }
                        }
                    }

                    await _con.CloseAsync();
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


        public async Task<int> NewOffer(Offer offer)
        {
            try
            {
                int offerId = 0;
                using (SqlConnection con = new SqlConnection(this._conString))
                {
                    using (SqlCommand cmd = new SqlCommand("InsertNewOffer", con))
                    {
                        await con.OpenAsync();
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@offerName", offer.Name);
                        cmd.Parameters.AddWithValue("@description", offer.Description);
                        cmd.Parameters.AddWithValue("@isRoomOffer", offer.IsRoomOffer);
                        cmd.Parameters.AddWithValue("@validFrom", offer.ValidFrom);
                        cmd.Parameters.AddWithValue("@validTo", offer.ValidTo);
                        cmd.Parameters.AddWithValue("@price", offer.Price);
                        cmd.Parameters.AddWithValue("@discountPercentage", offer.DiscountPercentage);
                        cmd.Parameters.AddWithValue("@createdBy", offer.CreatedBy);

                        offerId = (int?)(await cmd.ExecuteScalarAsync()) ?? 0;
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

        public async Task<bool> ChangeActiveStatus(bool status, int id)
        {
            try
            {
                var query = "UPDATE Offers SET IsActive = @status WHERE OfferId = @Id";

                using (SqlConnection con = new SqlConnection(this._conString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        await con.OpenAsync();

                        cmd.Parameters.AddWithValue("@Id", id);
                        cmd.Parameters.AddWithValue("@status", status);

                        await cmd.ExecuteNonQueryAsync();

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

        public async void UpdateOffer(Offer offer, int offerId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(this._conString))
                {
                    using (SqlCommand cmd = new SqlCommand("UpdateOffer", con))
                    {
                        await con.OpenAsync();

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

                        await cmd.ExecuteNonQueryAsync();
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

        public async Task<bool> DeleteOffer(int offerId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(this._conString))
                {
                    using (SqlCommand cmd = new SqlCommand("DeleteOfferWithPics", con))
                    {
                        await con.OpenAsync();

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@offerId", offerId);

                        await cmd.ExecuteNonQueryAsync();

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
