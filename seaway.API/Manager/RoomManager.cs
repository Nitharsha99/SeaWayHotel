using seaway.API.Configurations;
using seaway.API.Models;
using System.Data;
using System.Data.SqlClient;

namespace seaway.API.Manager
{
    public class RoomManager
    {
        private readonly ILogger<LogHandler> _logger;
        private readonly IConfiguration _configuration;
        string _conString;

        public RoomManager(IConfiguration configuration, ILogger<LogHandler> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _conString = _configuration.GetConnectionString("DefaultConnection");
        }

        public List<Room> GetRooms()
        {
            try
            {
                List<Room> roomList = new List<Room>();
                List<PicDocument> pics = new List<PicDocument>();

                using (SqlConnection _con = new SqlConnection(this._conString))
                {
                    SqlCommand command = _con.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "GetAllRoomsWithPicDetails";                

                    _con.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var price = reader["Price"];
                            var discountPer = reader["DiscountPercent"];
                            if (reader["DiscountPercent"] == DBNull.Value)
                            {
                                discountPer = 0.0;
                            }
                            
                            var discountAmount = reader["DiscountPrice"];
                            if(reader["DiscountPrice"] == DBNull.Value)
                            {
                                discountAmount = 0.0;
                            }

                            Room room = new Room
                            {
                                RoomId = (int)reader["RoomId"],
                                RoomName = reader["RoomName"].ToString(),
                                GuestCountMax = (int)reader["CountOfMaxGuest"],
                                Price = Convert.ToDouble(price),
                                DiscountPercentage = Convert.ToDouble(discountPer),
                                DiscountAmount = Convert.ToDouble(discountAmount)
                            };

                            if (reader["PicName"] != DBNull.Value)
                            {
                                byte[] picValueInByte = (byte[])reader["PicValue"];
                                string val = Convert.ToBase64String(picValueInByte);


                                PicDocument document = new PicDocument
                                {
                                    PicName = reader["PicName"].ToString(),
                                    PicType = reader["PicType"].ToString(),
                                    PicTypeId = (int)reader["PicTypeId"],
                                    CloudinaryPublicId = reader["CloudinaryPublicId"].ToString(),
                                    PicValue = val
                                };

                                if (room.RoomPics == null)
                                {
                                    room.RoomPics = new List<PicDocument>();
                                }

                                room.RoomPics.Add(document);

                            }

                            roomList.Add(room);
                        }
                    }

                    _con.Close();

                    _logger.LogTrace("SuccessFully All Room Data retrieved");

                    return roomList;
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(" Warning -- " + ex.Message);
                throw;
            }
        }

        public void NewRoom(Room room)
        {
            try
            {
                using(SqlConnection con = new SqlConnection(this._conString))
                {
                    using(SqlCommand cmd = new SqlCommand("InsertNewRoom", con))
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@roomName", room.RoomName);
                        cmd.Parameters.AddWithValue("@guestCountMax", room.GuestCountMax);
                        cmd.Parameters.AddWithValue("@price", room.Price);
                        cmd.Parameters.AddWithValue("@discountPercentage", room.DiscountPercentage);
                        cmd.Parameters.AddWithValue("@isActive", room.IsActive);

                        cmd.ExecuteNonQuery();
                    }
                }

                _logger.LogTrace("SuccessFully created new room");
            }
            catch(Exception ex)
            {
                _logger.LogWarning("Warning at Insert New Room : " + ex.Message);
                throw;
            }
        }
    }
}
