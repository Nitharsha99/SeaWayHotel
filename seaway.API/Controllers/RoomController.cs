using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using seaway.API.Configurations;
using seaway.API.Manager;
using seaway.API.Models;
using seaway.API.Models.Enum;
using seaway.API.Models.ViewModels;

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

        [HttpPost]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> NewRoom(Room room)
        {
            try
            {
                bool isNumberExist = false;
                if (room.RoomNumber == null || room.RoomTypeId == 0)
                {
                    return BadRequest(DisplayMessages.NullInput);
                }
                else
                {
                    isNumberExist = await _roomManager.IsNumberExist(room.RoomNumber);

                    if (isNumberExist)
                    {
                        return BadRequest(DisplayMessages.ExistNumberError);
                    }
                    else
                    {
                        bool isCreated = await _roomManager.NewRoom(room);

                        string responseBody = JsonConvert.SerializeObject(room);
                        string requestUrl = HttpContext.Request.Path.ToString();

                        _log.setLogTrace(new HttpRequestMessage(), new HttpResponseMessage(), responseBody, requestUrl);

                        if (!isCreated)
                        {
                            return StatusCode(500, "Internal Server Error");
                        }
                        else
                        {
                            return Ok(room);
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                _logger.LogError(LogMessages.InsertDataError + ex.Message);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPut]
        [Route("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateRoom(Room room, int Id)
        {
            try
            {
                bool isNumberExist = false;
                bool isNumberChange = false;

                if (Id > 0)
                {
                    Room oldRoom = await _roomManager.GetRoomById(Id);
                    if(oldRoom.RoomNumber != null)
                    {
                        if(room.RoomNumber != null)
                        {
                            isNumberChange = _roomManager.IsNumberChange(room.RoomNumber, oldRoom.RoomNumber);

                            if (isNumberChange)
                            {
                                isNumberExist = await _roomManager.IsNumberExist(room.RoomNumber);
                            }
                        }
                        if (isNumberExist)
                        {
                            return BadRequest(DisplayMessages.ExistNumberError);
                        }
                        else
                        {
                            room.Id = Id;
                            _roomManager.UpdateRoom(room);

                            string requestUrl = HttpContext.Request.Path.ToString();
                            string responseBody = JsonConvert.SerializeObject(room);

                            _log.setLogTrace(new HttpRequestMessage(), new HttpResponseMessage(), requestUrl, responseBody);

                            return Ok(room);
                        }
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
            catch(Exception ex)
            {
                _logger.LogError(LogMessages.InsertDataError + ex.Message);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpDelete]
        [Route("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteRoom([FromRoute] int Id)
        {
            try
            {
                if(Id > 0)
                {
                    bool isRoomRemove = false;

                    Room room = await _roomManager.GetRoomById(Id);

                    if(room.Id > 0)
                    {
                        isRoomRemove = await _roomManager.DeleteRoom(Id);

                        string requestUrl = HttpContext.Request.Path.ToString();
                        string responseBody = JsonConvert.SerializeObject(room);

                        _log.setLogTrace(new HttpRequestMessage(), new HttpResponseMessage(), requestUrl, responseBody);

                        if (isRoomRemove)
                        {
                            return Ok(room);
                        }
                        else
                        {
                            return BadRequest(DisplayMessages.DeletingError);
                        }
                    }
                    else
                    {
                        return NotFound(DisplayMessages.EmptyExistData);
                    }
                }
                else
                {
                    _logger.LogError(LogMessages.InvalidIdError);
                    return BadRequest(DisplayMessages.InvalidId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(LogMessages.DeleteRoomCategoryError + ex.Message);
                return StatusCode(500, "Internal Server Error");
            }
        }


    }
}
