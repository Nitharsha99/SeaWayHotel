using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using seaway.API.Configurations;
using seaway.API.Manager;
using seaway.API.Models;
using seaway.API.Models.ViewModels;

namespace seaway.API.Controllers
{
    [Route("api/Bookings")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly ILogger<LogHandler> _logger;
        private readonly IConfiguration _configuration;
        private readonly BookingManager _bookingManager;
        LogHandler _log;

        public BookingController(ILogger<LogHandler> logger, IConfiguration configuration, BookingManager bookingManager, LogHandler log)
        {
            _logger = logger;
            _configuration = configuration;
            _bookingManager = bookingManager;
            _log = log;
        }

        [Route("")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllBookings()
        {
            try
            {
                List<BookingListView> bookingList = await _bookingManager.GetAllBookings();

                string responseBody = JsonConvert.SerializeObject(bookingList);

                string requestUrl = HttpContext.Request.Path.ToString();

                _log.setLogTrace(new HttpRequestMessage(), new HttpResponseMessage(), responseBody, requestUrl);

                return Ok(bookingList);
            }
            catch (Exception ex)
            {
                _logger.LogError(LogMessages.GetBookingDataError + ex.Message);
                return StatusCode(500, "Internal Server Error");
            }
        }

    }
}
