using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using seaway.API.Configurations;
using seaway.API.Manager;
using seaway.API.Models;
using seaway.API.Models.ViewModels;
using System.Diagnostics;

namespace seaway.API.Controllers
{
    [Route("api/Bookings")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly ILogger<LogHandler> _logger;
        private readonly IConfiguration _configuration;
        private readonly BookingManager _bookingManager;
        private readonly CustomerManager _customerManager;
        LogHandler _log;

        public BookingController(ILogger<LogHandler> logger, IConfiguration configuration, BookingManager bookingManager, LogHandler log, CustomerManager customerManager)
        {
            _logger = logger;
            _configuration = configuration;
            _bookingManager = bookingManager;
            _customerManager = customerManager;
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

        [HttpPost]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> NewBooking(BookingAdd bookingAdd)
        {
            try
            {
                if(bookingAdd.PassportNo != null || bookingAdd.NIC != null)
                {
                    var existCustomer = await _customerManager.GetByUniqueId(bookingAdd.NIC ?? bookingAdd.PassportNo)
                                   .ConfigureAwait(false);

                    Bookings bookings = new Bookings
                    {
                        BookingDate = bookingAdd.BookingDate,
                        CheckIn = bookingAdd.CheckIn,
                        CheckOut = bookingAdd.CheckOut,
                        GuestCount = bookingAdd.GuestCount,
                        RoomCount = bookingAdd.RoomCount
                    };

                    if (existCustomer.Id == 0)
                    {
                        Customer customer = new Customer
                        {
                            Name = bookingAdd.Name,
                            NIC = bookingAdd.NIC,
                            PassportNo = bookingAdd.PassportNo,
                            ContactNo = bookingAdd.ContactNo,
                            Email_add = bookingAdd.Email_add
                        };

                        bookings.CustomerId = await _customerManager.PostCustomer(customer);
                    }
                    else
                    {
                        bookings.CustomerId = existCustomer.Id;
                    }

                    bool booked = await _bookingManager.NewBooking(bookings);

                    if (booked)
                    {
                        return Ok(true);
                    }
                    else
                    {
                        return StatusCode(400, "Booking Failed");
                    }
                }
                else
                {
                    return StatusCode(401, "Invalid Identity Id");
                }
                
            }
            catch (Exception ex)
            {
                _logger.LogError(LogMessages.InsertDataError + ex.Message);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet]
        [Route("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetBookingById([FromRoute] int Id)
        {
            try
            {
                if(Id > 0)
                {
                    BookingDetails bookingDetail = await _bookingManager.GetBookingById(Id).ConfigureAwait(false);

                    string responseBody = JsonConvert.SerializeObject(bookingDetail);

                    string requestUrl = HttpContext.Request.Path.ToString();

                    _log.setLogTrace(new HttpRequestMessage(), new HttpResponseMessage(), responseBody, requestUrl);

                    if(bookingDetail.Id > 0)
                    {
                        return Ok(bookingDetail);
                    }
                    else
                    {
                        return NotFound(DisplayMessages.EmptyExistData + Id);
                    }
                }
                else
                {
                    return BadRequest(DisplayMessages.InvalidId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(LogMessages.FindDataByIdError + ex.Message);
                return StatusCode(500, "Internal Server Error");
            }
        }

    }
}
