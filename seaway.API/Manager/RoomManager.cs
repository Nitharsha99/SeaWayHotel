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
