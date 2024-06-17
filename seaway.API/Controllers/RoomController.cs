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
        public IActionResult FindRoomById([FromRoute] int roomId)
        {
            try
            {
                Room room = _roomManager.GetRoomById(roomId);

                string responseBody = JsonConvert.SerializeObject(room);

                string requestUrl = HttpContext.Request.Path.ToString();

                _log.setLogTrace(new HttpRequestMessage(), new HttpResponseMessage(), responseBody, requestUrl);
                return Ok(room);
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
                            pic.CloudinaryPublicId = item.CloudinaryPublicId;

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

        [HttpPut]
        [Route("{roomId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateRoom(RoomWithPicModel room, [FromRoute]int roomId)
        {
            try
            {
                if (roomId != 0)
                {
                    Room oldRoom = _roomManager.GetRoomById(roomId);
                    if (oldRoom.RoomName != null)
                    {
                        Room updateRoom = new Room
                        {
                            RoomName = room.RoomName,
                            GuestCountMax = room.GuestCountMax,
                            Price = room.Price,
                            DiscountPercentage = room.DiscountPercentage,
                        };
                        _roomManager.UpdateRoom(updateRoom, roomId);

                        PicDocument pic = new PicDocument
                        {
                            PicType = "Room",
                            PicTypeId = roomId
                        };

                        if (room?.roomPics?.Length > 0)
                        {
                            foreach (var item in room.roomPics)
                            {
                                pic.PicValue = item.PicValue;
                                pic.PicName = item.PicName;
                                pic.CloudinaryPublicId = item.CloudinaryPublicId;

                                _docManager.UploadImage(pic);
                            }
                        }

                        string requestUrl = HttpContext.Request.Path.ToString();
                        string responseBody = JsonConvert.SerializeObject(room);

                        _log.setLogTrace(new HttpRequestMessage(), new HttpResponseMessage(), requestUrl, responseBody);

                    }
                    else
                    {
                        return BadRequest("Invalid RoomId");
                    }
                }

                return Ok(room);
            }
            catch (Exception ex)
            {
                _logger.LogError("An exception occurred while updating room data : " + ex.Message);
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete]
        [Route("image")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteAsset([FromQuery] string ids)
        {
            try
            {
                List<string> idArray = new List<string>();
                idArray = ids.Split(',').ToList();
                bool IsRemoveFromCLoudinary = false;

                IsRemoveFromCLoudinary = _docManager.DeleteAssetFromCloudinary(idArray).Result;

                if (IsRemoveFromCLoudinary)
                {
                    string picType = "Room";
                    _docManager.DeleteImageFromDB(idArray, picType);
                }

                string requestUrl = HttpContext.Request.Path.ToString();
                string responseBody = JsonConvert.SerializeObject(ids);

                _log.setLogTrace(new HttpRequestMessage(), new HttpResponseMessage(), requestUrl, responseBody);
                return Ok("Deleted " + responseBody);
            }
            catch (Exception ex)
            {
                _logger.LogError("An exception occurred while deleting room pictures : " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteRoom([FromRoute] int Id)
        {
            try
            {
                Room room = new Room();
                bool isRoomRemove = false;
                List<string> publicIds = new List<string>();
                bool IsRemoveFromCLoudinary = false;

                room = _roomManager.GetRoomById(Id);

                if (room.RoomId != null)
                {
                    if(room?.RoomPics?.Count > 0)
                    {
                        foreach(var pic in room.RoomPics)
                        {
                            publicIds.Add(pic.CloudinaryPublicId ?? "");
                        }
                       IsRemoveFromCLoudinary = _docManager.DeleteAssetFromCloudinary(publicIds).Result;

                        if (IsRemoveFromCLoudinary)
                        {
                            isRoomRemove = _roomManager.DeleteRoom(Id);
                        }
                    }
                    isRoomRemove = _roomManager.DeleteRoom(Id);

                    string requestUrl = HttpContext.Request.Path.ToString();
                    string responseBody = JsonConvert.SerializeObject(room);

                    _log.setLogTrace(new HttpRequestMessage(), new HttpResponseMessage(), requestUrl, responseBody);

                    if (isRoomRemove)
                    {
                        return Ok(room);
                    }
                    else
                    {
                        return BadRequest("Issue in deleting process");
                    }
                }
                else
                {
                    return BadRequest("Room is not exist for this Id");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("An exception occurred while deleting room : " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

    }
}
