using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using seaway.API.Configurations;
using seaway.API.Manager;
using seaway.API.Models;

namespace seaway.API.Controllers
{
    [Route("api/Room")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly ILogger<LogHandler> _logger;
        private readonly IConfiguration _configuration;
        private readonly RoomManager _roomManager;
        LogHandler _log;
        public RoomController(ILogger<LogHandler> logger, IConfiguration configuration, LogHandler log, RoomManager roomManager)
        {
            _logger = logger;
            _configuration = configuration;
            _log = log;
            _roomManager = roomManager;
        }

        [Route("")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllRooms()
        {
            try
            {
                List<Room> rooms = await _roomManager.GetAllRooms();

                string responseBody = JsonConvert.SerializeObject(rooms);

                string requestUrl = HttpContext.Request.Path.ToString();

                _log.setLogTrace(new HttpRequestMessage(), new HttpResponseMessage(), responseBody, requestUrl);

                return Ok(rooms);
            }
            catch (Exception ex)
            {
                _logger.LogError(LogMessages.GetRoomDataError + ex.Message);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet]
        [Route("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetRoomById([FromRoute] int Id)
        {
            try
            {
                if (Id > 0)
                {
                    Room room = await _roomManager.GetRoomById(Id);

                    string responseBody = JsonConvert.SerializeObject(room);

                    string requestUrl = HttpContext.Request.Path.ToString();

                    _log.setLogTrace(new HttpRequestMessage(), new HttpResponseMessage(), responseBody, requestUrl);

                    if (room.Id > 0)
                    {
                        return Ok(room);
                    }
                    else
                    {
                        return NotFound(DisplayMessages.EmptyExistData);
                    }
                }
                else
                {
                    return BadRequest(DisplayMessages.InvalidId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(LogMessages.FindDataByIdError + Id + " : " + ex.Message);
                return StatusCode(500, "Internal Server Error");
            }
        }


    }
}
