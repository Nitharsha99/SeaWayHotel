using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using seaway.API.Configurations;
using seaway.API.Manager;
using seaway.API.Models;
using System.Diagnostics;

namespace seaway.API.Controllers
{
    [Route("api/Room")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly ILogger<LogHandler> _logger;
        private readonly IConfiguration _configuration;
        LogHandler _log;
        RoomManager _roomManager;

        public RoomController(ILogger<LogHandler> logger, IConfiguration configuration, LogHandler log, RoomManager roomManager)
        {
            _logger = logger;
            _configuration = configuration;
            _log = log;
            _roomManager = roomManager;
        }

        [HttpPost]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PostNewRoom([FromForm] Room room)
        {
            try
            {
                if(room != null)
                {
                    _roomManager.NewRoom(room);

                    string requestUrl = HttpContext.Request.Path.ToString();
                    string responseBody = JsonConvert.SerializeObject(room);

                    _log.setLogTrace(new HttpRequestMessage(), new HttpResponseMessage(), requestUrl, responseBody);
                }

                return Ok(room);
            }
            catch (Exception ex)
            {
                _logger.LogError("An exception occurred while inserting activity data : " + ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
