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
    [Route("api/Room")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly ILogger<LogHandler> _logger;
        private readonly IConfiguration _configuration;
        LogHandler _log;
        RoomManager _roomManager;
        PicDocumentManager _docManager;

        public RoomController(ILogger<LogHandler> logger, IConfiguration configuration, LogHandler log, RoomManager roomManager, PicDocumentManager docManager)
        {
            _logger = logger;
            _configuration = configuration;
            _log = log;
            _roomManager = roomManager;
            _docManager = docManager;
        }

        [Route("")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetAllRooms()
        {
            try
            {
                List<Room> rooms = _roomManager.GetRooms();

                string responseBody = JsonConvert.SerializeObject(rooms);

                string requestUrl = HttpContext.Request.Path.ToString();

                _log.setLogTrace(new HttpRequestMessage(), new HttpResponseMessage(), responseBody, requestUrl);

                return Ok(rooms);
            }
            catch (Exception ex)
            {
                _logger.LogError("An exception occurred while get all room data : " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("{roomId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult FindRoomById(int roomId)
        {
            try
            {
                List<Room> rooms = _roomManager.GetRoomById(roomId);

                string responseBody = JsonConvert.SerializeObject(rooms);

                string requestUrl = HttpContext.Request.Path.ToString();

                _log.setLogTrace(new HttpRequestMessage(), new HttpResponseMessage(), responseBody, requestUrl);
                return Ok(rooms);
            }
            catch(Exception ex)
            {
                _logger.LogError("An exception occurred while get room data with Id = " + roomId + " : " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PostNewRoom(RoomWithPicModel room)
        {
            try
            {
                if(room != null)
                {
                    Room r = new Room
                    {
                        RoomName = room.RoomName,
                        GuestCountMax = room.GuestCountMax,
                        Price = room.Price,
                        DiscountPercentage = room.DiscountPercentage
                    };
                    int roomId = _roomManager.NewRoom(room);

                    PicDocument pic = new PicDocument
                    {
                        PicType = "Room",
                        PicTypeId = roomId
                    };

                    if(room?.roomPics?.Length > 0)
                    {
                        foreach(var item in room.roomPics)
                        {
                            pic.PicValue = item.PicValue;
                            pic.PicName = item.PicName;
                            pic.CloudinaryPublicId = item.PublicId;

                            _docManager.UploadImage(pic);
                        }
                    }

                    string requestUrl = HttpContext.Request.Path.ToString();
                    string responseBody = JsonConvert.SerializeObject(room);

                    _log.setLogTrace(new HttpRequestMessage(), new HttpResponseMessage(), requestUrl, responseBody);
                }

                return Ok(room);
            }
            catch (Exception ex)
            {
                _logger.LogError("An exception occurred while inserting room data : " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        //[HttpPut]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public IActionResult UpdateRoom(RoomWithPicModel room, int roomId)
        //{
        //    try
        //    {
        //        if((room != null) && (roomId != 0))
        //        {
                   
        //        }

        //        return Ok(room);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError("An exception occurred while updating room data : " + ex.Message);
        //        return BadRequest(ex.Message);
        //    }
        //}

    }
}
