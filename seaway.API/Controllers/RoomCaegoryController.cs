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
    [Route("api/RoomCategory")]
    [ApiController]
    public class RoomCaegoryController : ControllerBase
    {
        private readonly ILogger<LogHandler> _logger;
        private readonly IConfiguration _configuration;
        LogHandler _log;
        RoomCategoryManager _roomCategoryManager;
        PicDocumentManager _docManager;

        public RoomCaegoryController(ILogger<LogHandler> logger, IConfiguration configuration, LogHandler log, RoomCategoryManager roomCategoyManager, PicDocumentManager docManager)
        {
            _logger = logger;
            _configuration = configuration;
            _log = log;
            _roomCategoryManager = roomCategoyManager;
            _docManager = docManager;
        }

        [Route("")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetAllRoomCategories()
        {
            try
            {
                List<RoomCategory> categories = _roomCategoryManager.GetRooms();

                string responseBody = JsonConvert.SerializeObject(categories);

                string requestUrl = HttpContext.Request.Path.ToString();

                _log.setLogTrace(new HttpRequestMessage(), new HttpResponseMessage(), responseBody, requestUrl);

                return Ok(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(LogMessages.GetRoomDataError + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult FindCategoryById([FromRoute] int Id)
        {
            try
            {
                if(Id > 0)
                {
                    RoomCategory Category = _roomCategoryManager.GetRoomById(Id);

                    string responseBody = JsonConvert.SerializeObject(Category);

                    string requestUrl = HttpContext.Request.Path.ToString();

                    _log.setLogTrace(new HttpRequestMessage(), new HttpResponseMessage(), responseBody, requestUrl);
                    return Ok(Category);
                }
                else
                {
                    _logger.LogError(LogMessages.InvalidIdError);
                    return BadRequest(DisplayMessages.InvalidId);
                }

            }
            catch(Exception ex)
            {
                _logger.LogError(LogMessages.FindRoomByIdError + Id + " : " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PostNewCategory(RoomWithPicModel Category)
        {
            try
            {
                if(Category != null)
                {
                    RoomCategory r = new RoomCategory
                    {
                        RoomName = Category.RoomName,
                        GuestCountMax = Category.GuestCountMax,
                        Price = Category.Price,
                        DiscountPercentage = Category.DiscountPercentage
                    };
                    int roomId = _roomCategoryManager.NewRoom(Category);

                    PicDocument pic = new PicDocument
                    {
                        PicType = "Room",
                        PicTypeId = roomId
                    };

                    if(Category?.roomPics?.Length > 0)
                    {
                        foreach(var item in Category.roomPics)
                        {
                            pic.PicValue = item.PicValue;
                            pic.PicName = item.PicName;
                            pic.CloudinaryPublicId = item.CloudinaryPublicId;

                            _docManager.UploadImage(pic);
                        }
                    }

                    string requestUrl = HttpContext.Request.Path.ToString();
                    string responseBody = JsonConvert.SerializeObject(Category);

                    _log.setLogTrace(new HttpRequestMessage(), new HttpResponseMessage(), requestUrl, responseBody);

                    return Ok(Category);
                }
                else
                {
                    _logger.LogError(LogMessages.EmptyDataSetError);
                    return BadRequest();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(LogMessages.InsertDataError + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateRoomCategory(RoomWithPicModel category, [FromRoute]int Id)
        {
            try
            {
                if (Id > 0)
                {
                    RoomCategory oldRoom = _roomCategoryManager.GetRoomById(Id);
                    if (oldRoom.RoomName != null)
                    {
                        RoomCategory updateCategory = new RoomCategory
                        {
                            RoomName = category.RoomName,
                            GuestCountMax = category.GuestCountMax,
                            Price = category.Price,
                            DiscountPercentage = category.DiscountPercentage,
                        };
                        _roomCategoryManager.UpdateRoom(updateCategory, Id);

                        PicDocument pic = new PicDocument
                        {
                            PicType = "Room",
                            PicTypeId = Id
                        };

                        if (category?.roomPics?.Length > 0)
                        {
                            foreach (var item in category.roomPics)
                            {
                                pic.PicValue = item.PicValue;
                                pic.PicName = item.PicName;
                                pic.CloudinaryPublicId = item.CloudinaryPublicId;

                                _docManager.UploadImage(pic);
                            }
                        }

                        string requestUrl = HttpContext.Request.Path.ToString();
                        string responseBody = JsonConvert.SerializeObject(category);

                        _log.setLogTrace(new HttpRequestMessage(), new HttpResponseMessage(), requestUrl, responseBody);

                    }
                    else
                    {
                        return BadRequest();
                    }

                    return Ok(category);
                }
                else
                {
                    _logger.LogError(LogMessages.InvalidIdError);
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(LogMessages.UpdateDataError + ex.Message);
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
                if(ids == null)
                {
                    return NotFound();
                }
                else
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
                    return Ok("Deleted : " + responseBody);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(LogMessages.DeleteImageError + ex.Message);
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
                if(Id > 0)
                {
                    RoomCategory Category = new RoomCategory();
                    bool isRoomRemove = false;
                    List<string> publicIds = new List<string>();
                    bool IsRemoveFromCLoudinary = false;

                    Category = _roomCategoryManager.GetRoomById(Id);

                    if (Category.RoomCategoryId != null)
                    {
                        if (Category?.RoomPics?.Count > 0)
                        {
                            foreach (var pic in Category.RoomPics)
                            {
                                publicIds.Add(pic.CloudinaryPublicId ?? "");
                            }
                            IsRemoveFromCLoudinary = _docManager.DeleteAssetFromCloudinary(publicIds).Result;

                            if (IsRemoveFromCLoudinary)
                            {
                                isRoomRemove = _roomCategoryManager.DeleteRoom(Id);
                            }
                        }
                        isRoomRemove = _roomCategoryManager.DeleteRoom(Id);

                        string requestUrl = HttpContext.Request.Path.ToString();
                        string responseBody = JsonConvert.SerializeObject(Category);

                        _log.setLogTrace(new HttpRequestMessage(), new HttpResponseMessage(), requestUrl, responseBody);

                        if (isRoomRemove)
                        {
                            return Ok(Category);
                        }
                        else
                        {
                            return BadRequest("Issue in deleting process");
                        }
                    }
                    else
                    {
                        return NotFound("Invalid Id");
                    }
                }
                else
                {
                    _logger.LogError(LogMessages.InvalidIdError);
                    return BadRequest("Room is not exist for this Id");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(LogMessages.DeleteRoomError + ex.Message);
                return BadRequest(ex.Message);
            }
        }

    }
}
