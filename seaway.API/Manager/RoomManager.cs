using seaway.API.Configurations;
using seaway.API.Models;
using seaway.API.Models.ViewModels;
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
                            var roomId = (int)reader["RoomId"];
                            Room room = roomList.FirstOrDefault(r => r.RoomId == roomId) ?? new Room();

                            if (room.RoomId == null)
                            {
                                room = new Room
                                {
                                    RoomId = (int)reader["RoomId"],
                                    RoomName = reader["RoomName"].ToString(),
                                    GuestCountMax = (int)reader["CountOfMaxGuest"],
                                    Price = Convert.ToDouble(reader["Price"]),
                                    DiscountPercentage = reader["DiscountPercent"] == DBNull.Value ? 0.0 : Convert.ToDouble(reader["DiscountPercent"]),
                                    DiscountAmount = reader["DiscountPrice"] == DBNull.Value ? 0.0 : Convert.ToDouble(reader["DiscountPrice"]),
                                };

                                roomList.Add(room);
                            }

                            if (room.RoomId == (int)reader["PicTypeId"] && reader["PicName"] != DBNull.Value)
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

        public int NewRoom(RoomWithPicModel room)
        {
            try
            {
                int roomId = 0;
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

                        roomId = (int)cmd.ExecuteScalar();
                    }
                }

                _logger.LogTrace("SuccessFully created new room");

                return roomId;
            }
            catch(Exception ex)
            {
                _logger.LogWarning("Warning at Insert New Room : " + ex.Message);
                throw;
            }
        }

        public Room GetRoomById(int roomId)
        {
            try
            {
                List<Room> roomList = new List<Room>();
                Room mainRoom = new Room();

                using (SqlConnection _con = new SqlConnection(this._conString))
                {
                    _con.Open();
                    using(var cmd = new SqlCommand("GetAllRoomsWithPicDetails", _con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@roomId", roomId);

                        using(var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var Id = (int)reader["RoomId"];
                                var type = reader["PicType"].ToString();
                                var name = reader["PicName"].ToString();
                                Room room = roomList.FirstOrDefault(r => r.RoomId == Id) ?? new Room();

                                if (room.RoomId == null)
                                {
                                    room = new Room
                                    {
                                        RoomId = (int)reader["RoomId"],
                                        RoomName = reader["RoomName"].ToString(),
                                        GuestCountMax = (int)reader["CountOfMaxGuest"],
                                        Price = Convert.ToDouble(reader["Price"]),
                                        DiscountPercentage = reader["DiscountPercent"] == DBNull.Value ? 0.0 : Convert.ToDouble(reader["DiscountPercent"]),
                                        DiscountAmount = reader["DiscountPrice"] == DBNull.Value ? 0.0 : Convert.ToDouble(reader["DiscountPrice"]),
                                    };

                                    roomList.Add(room);
                                }

                                var pname = reader["PicName"].ToString();
                                if (reader["PicName"] != DBNull.Value && room.RoomId == (int)reader["PicTypeId"])
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
                            }
                        }
                    }

                    _con.Close();
                    _logger.LogTrace("SuccessFully All Room Data retrieved");
                    mainRoom = roomList.FirstOrDefault() ?? new Room();

                }
                return mainRoom;
            }
            catch(Exception e)
            {
                _logger.LogWarning(" Warning -- " + e.Message);
                throw;
            }
        }

        public async Task<bool> DeleteRoom(int roomId)
        {
            try
            {
                var query = "DELETE FROM Room WHERE RoomId = @Id";

                using (SqlConnection con = new SqlConnection(this._conString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Id", roomId);

                        con.Open();
                        cmd.ExecuteNonQuery();

                        _logger.LogTrace("Sucessfully Deleted Room of Id --> " + roomId + "From Database");

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
