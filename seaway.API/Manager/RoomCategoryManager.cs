using seaway.API.Configurations;
using seaway.API.Models;
using seaway.API.Models.Enum;
using seaway.API.Models.ViewModels;
using System.Data;
using System.Data.SqlClient;

namespace seaway.API.Manager
{
    public class RoomCategoryManager
    {
        private readonly ILogger<LogHandler> _logger;
        private readonly IConfiguration _configuration;
        string _conString;

        public RoomCategoryManager(IConfiguration configuration, ILogger<LogHandler> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _conString = _configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<RoomCategory>> GetRoomCategories()
        {
            try
            {
                List<RoomCategory> categoryList = new List<RoomCategory>();

                using (SqlConnection _con = new SqlConnection(this._conString))
                {
                    SqlCommand command = _con.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "GetAllRoomCategories";

                    await _con.OpenAsync();

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                                RoomCategory category = new RoomCategory
                                {
                                    CategoryId = (int)reader["CategoryId"],
                                    RoomName = reader["RoomName"].ToString(),
                                    GuestCountMax = (int)reader["CountOfMaxGuest"],
                                    Price = Convert.ToDouble(reader["Price"]),
                                    DiscountPercentage = reader["DiscountPercent"] == DBNull.Value ? 0.0 : Convert.ToDouble(reader["DiscountPercent"]),
                                    DiscountAmount = reader["DiscountPrice"] == DBNull.Value ? 0.0 : Convert.ToDouble(reader["DiscountPrice"]),
                                    Created = Convert.ToDateTime(reader["Created"]),
                                    CreatedBy = reader["CreatedBy"].ToString(),
                                    Updated = Convert.ToDateTime(reader["Updated"]),
                                    UpdatedBy = reader["UpdatedBy"].ToString()
                                };

                            categoryList.Add(category);
                            
                        }
                    }

                    await _con.CloseAsync();

                    _logger.LogTrace("SuccessFully All Room Data retrieved");

                    return categoryList;
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(" Warning -- " + ex.Message);
                throw;
            }
        }

        public async Task<int> NewRoomCategory(RoomCategoryWithPicModel category)
        {
            try
            {
                int categoryId = 0;
                using(SqlConnection con = new SqlConnection(this._conString))
                {
                    using(SqlCommand cmd = new SqlCommand("InsertNewRoomCategory", con))
                    {
                        await con.OpenAsync();
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@roomName", category.RoomName);
                        cmd.Parameters.AddWithValue("@guestCountMax", category.GuestCountMax);
                        cmd.Parameters.AddWithValue("@price", category.Price);
                        cmd.Parameters.AddWithValue("@discountPercentage", category.DiscountPercentage);
                        cmd.Parameters.AddWithValue("@createdBy", category.CreatedBy);

                        categoryId = (int?)(await cmd.ExecuteScalarAsync()) ?? 0; ;
                    }
                }

                _logger.LogTrace("SuccessFully created new room");

                return categoryId;
            }
            catch(Exception ex)
            {
                _logger.LogWarning("Warning at Insert New Room : " + ex.Message);
                throw;
            }
        }

        public async Task<RoomCategory> GetRoomCategoryById(int categoryId)
        {
            try
            {
                List<RoomCategory> categoryList = new List<RoomCategory>();
                RoomCategory maincategory = new RoomCategory();

                using (SqlConnection _con = new SqlConnection(this._conString))
                {
                    await _con.OpenAsync();
                    using(var cmd = new SqlCommand("GetAllRoomCategoryWithPicDetails", _con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@categoryId", categoryId);

                        using(var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var Id = (int)reader["CategoryId"];
                                RoomCategory category = categoryList.FirstOrDefault(r => r.CategoryId == Id) ?? new RoomCategory();

                                if (category.CategoryId == null)
                                {
                                    category = new RoomCategory
                                    {
                                        CategoryId = (int)reader["CategoryId"],
                                        RoomName = reader["RoomName"].ToString(),
                                        GuestCountMax = (int)reader["CountOfMaxGuest"],
                                        Price = Convert.ToDouble(reader["Price"]),
                                        DiscountPercentage = reader["DiscountPercent"] == DBNull.Value ? 0.0 : Convert.ToDouble(reader["DiscountPercent"]),
                                        DiscountAmount = reader["DiscountPrice"] == DBNull.Value ? 0.0 : Convert.ToDouble(reader["DiscountPrice"]),
                                        Created = Convert.ToDateTime(reader["Created"]),
                                        CreatedBy = reader["CreatedBy"].ToString(),
                                        Updated = Convert.ToDateTime(reader["Updated"]),
                                        UpdatedBy = reader["UpdatedBy"].ToString()
                                    };

                                    categoryList.Add(category);
                                }

                                if (reader["PicName"] != DBNull.Value)
                                {
                                    byte[] picValueInByte = (byte[])reader["PicValue"];
                                    string val = Convert.ToBase64String(picValueInByte);

                                    PicDocument document = new PicDocument
                                    {
                                        PicName = reader["PicName"].ToString(),
                                        PicType = (PicType)reader["PicType"],
                                        PicTypeId = (int)reader["PicTypeId"],
                                        CloudinaryPublicId = reader["CloudinaryPublicId"].ToString(),
                                        PicValue = val
                                    };

                                    if (category.RoomPics == null)
                                    {
                                        category.RoomPics = new List<PicDocument>();
                                    }

                                    category.RoomPics.Add(document);
                                }
                            }
                        }
                    }

                    await _con.CloseAsync();
                    _logger.LogTrace("SuccessFully All Room Data retrieved");
                    maincategory = categoryList.FirstOrDefault() ?? new RoomCategory();

                }
                return maincategory;
            }
            catch(Exception e)
            {
                _logger.LogWarning(" Warning -- " + e.Message);
                throw;
            }
        }

        public async void UpdateRoomCategory(RoomCategory category, int categoryId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(this._conString))
                {
                    using (SqlCommand cmd = new SqlCommand("UpdateRoomCategory", con))
                    {
                        await con.OpenAsync();
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@categoryId", categoryId);
                        cmd.Parameters.AddWithValue("@roomName", category.RoomName);
                        cmd.Parameters.AddWithValue("@guestCount", category.GuestCountMax);
                        cmd.Parameters.AddWithValue("@price", category.Price);
                        cmd.Parameters.AddWithValue("@discountPercent", category.DiscountPercentage);
                        cmd.Parameters.AddWithValue("@updatedBy", category.UpdatedBy);

                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                _logger.LogTrace("SuccessFully updated the room");
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Warning at Update the " + category.RoomName + " : " + ex.Message);
                throw;
            }
        }

        public async Task<bool> DeleteRoomCategory(int categoryId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(this._conString))
                {
                    using (SqlCommand cmd = new SqlCommand("DeleteRoomCategoryWithPics", con))
                    {
                        await con.OpenAsync();

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@categoryId", categoryId);

                        await cmd.ExecuteNonQueryAsync();

                        _logger.LogTrace("Sucessfully Deleted Room of Id --> " + categoryId + "From Database");

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

        public async Task<bool> IsUsernameExist(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }
            else
            {
                var result = (await GetRoomCategories())
                    .Where(a => a.RoomName.Trim().ToLower() == name.Trim().ToLower())
                    .ToList();

                if (result.Count() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsNameChange(string inputName, string oldName)
        {
            if (string.IsNullOrEmpty(inputName) || string.IsNullOrEmpty(oldName)) { return false; }
            else
            {
                bool isNameChange = inputName.Trim().ToLower() != oldName.Trim().ToLower();
                return isNameChange;
            }
        }

    }
}
