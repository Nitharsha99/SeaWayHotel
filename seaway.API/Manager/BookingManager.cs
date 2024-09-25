using seaway.API.Configurations;
using seaway.API.Models;
using System.Data.SqlClient;
using System.Data;
using seaway.API.Models.ViewModels;

namespace seaway.API.Manager
{
    public class BookingManager
    {
        private readonly ILogger<LogHandler> _logger;
        private readonly IConfiguration _configuration;
        string _conString;

        public BookingManager(ILogger<LogHandler> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _conString = _configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<BookingListView>> GetAllBookings()
        {
            try
            {
                List<BookingListView> bookingList = new List<BookingListView>();
                var query = "SELECT * FROM Bookings B JOIN Customer C ON B.CustomerId = C.CustomerId";

                using(SqlConnection con = new SqlConnection(this._conString))
                {
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

                    await con.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            BookingListView book = new BookingListView
                            {
                                BookingId = (int)reader["BookingId"],
                                BookingDate = Convert.ToDateTime(reader["BookingDate"]),
                                CheckIn = Convert.ToDateTime(reader["CheckinDateTime"]),
                                CheckOut = Convert.ToDateTime(reader["CheckoutDateTime"]),
                                CustomerId = (int)reader["CustomerId"],
                                GuestCount = (int)reader["GuestCount"],
                                RoomCount = (int)reader["RoomCount"],
                                Name = reader["Name"].ToString(),
                                Created = Convert.ToDateTime(reader["Created"]),
                                Updated = Convert.ToDateTime(reader["Updated"]),
                            };

                            bookingList.Add(book);
                        }
                    }

                    await con.CloseAsync();
                }

                return bookingList;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(" Warning -- " + ex.Message);
                throw;
            }
        }

        public async Task<List<Bookings>> GetBookingsByRoomId(int id)
        {
            try
            {
                List<Bookings> bookings = new List<Bookings>();

                using (SqlConnection _con = new SqlConnection(this._conString))
                {
                    SqlCommand command = _con.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "BookingDetailsByRoomId";
                    command.Parameters.AddWithValue("@roomId", id);

                    await _con.OpenAsync();

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Bookings book = new Bookings
                            {
                                BookingId = (int)reader["BookingId"],
                                BookingDate = Convert.ToDateTime(reader["BookingDate"]),
                                CheckIn = Convert.ToDateTime(reader["CheckinDateTime"]),
                                CheckOut = Convert.ToDateTime(reader["CheckoutDateTime"])
                            };

                            bookings.Add(book);
                        }
                    }

                    await _con.CloseAsync();

                   // _logger.LogTrace(LogMessages.AllRoomsRetrieve);

                    return bookings;
                }
            }
            catch (Exception e)
            {
                _logger.LogWarning(" Warning -- " + e.Message);
                throw;
            }
        }
    }
}
