using seaway.API.Configurations;
using seaway.API.Models.ViewModels;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using seaway.API.Models;

namespace seaway.API.Manager
{
    public class RoomManager
    {
        private readonly ILogger<LogHandler> _logger;
        private readonly IConfiguration _configuration;
        private readonly RoomCategoryManager _roomCategoryManager;
        string _conString;
        public RoomManager(ILogger<LogHandler> logger, IConfiguration configuration, RoomCategoryManager roomCategoryManager)
        {
            _logger = logger;
            _configuration = configuration;
            _conString = _configuration.GetConnectionString("DefaultConnection");
            _roomCategoryManager = roomCategoryManager;
        }

        public async Task<List<Room>> GetAllRooms()
        {
            try
            {
                List<Room> rooms = new List<Room>();

                using (SqlConnection _con = new SqlConnection(this._conString))
                {
                    SqlCommand command = _con.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "GetAllRooms";

                    await _con.OpenAsync();

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Room r = new Room
                            {
                                Id = (int)reader["RoomId"],
                                RoomNumber = reader["RoomNumber"].ToString(),
                                RoomTypeId = (int)reader["RoomTypeId"],
                                Created = Convert.ToDateTime(reader["Created"]),
                                CreatedBy = reader["CreatedBy"].ToString(),
                                Updated = Convert.ToDateTime(reader["Updated"]),
                                UpdatedBy = reader["UpdatedBy"].ToString()
                            };

                            RoomCategory roomTypes = await _roomCategoryManager.GetRoomCategoryById(r.RoomTypeId ?? 0);
                            r.RoomType = roomTypes.RoomName;

                            rooms.Add(r);
                        }
                    }

                    await _con.CloseAsync();

                    _logger.LogTrace(LogMessages.AllRoomsRetrieve);

                    return rooms;
                }
            }
            catch (Exception e)
            {
                _logger.LogWarning(" Warning -- " + e.Message);
                throw;
            }
        }

        public async Task<Room> GetRoomById(int id)
        {
            try
            {
                List<Room> roomList = await GetAllRooms();

                var room = roomList.FirstOrDefault(a => a.Id == id) ?? new Room();

                return room;
            }
            catch(Exception e)
            {
                _logger.LogWarning(" Warning -- " + e.Message);
                throw;
            }
        }
    }
}
