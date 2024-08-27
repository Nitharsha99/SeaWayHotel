using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using seaway.API.Configurations;
using seaway.API.Manager;
using seaway.API.Models;
using seaway.API.Models.ViewModels;
using seaway.API.Models.Enum;

namespace seaway.API.Controllers
{
    [Route("api/Offer")]
    [ApiController]
    public class OfferController : ControllerBase
    {
        private readonly ILogger<LogHandler> _logger;
        private readonly IConfiguration _configuration;
        LogHandler _log;
        OfferManager _offerManager;
        PicDocumentManager _documentManager;

        public OfferController(ILogger<LogHandler> logger, IConfiguration configuration, LogHandler log, OfferManager offerManager, PicDocumentManager documentManager)
        {
            _configuration = configuration;
            _logger = logger;
            _log = log;
            _offerManager = offerManager;
            _documentManager = documentManager;
        }

        [Route("")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllOffers()
        {
            try
            {
                List<Offer> offers = await _offerManager.GetOffers();

                string responseBody = JsonConvert.SerializeObject(offers);

                string requestUrl = HttpContext.Request.Path.ToString();

                _log.setLogTrace(new HttpRequestMessage(), new HttpResponseMessage(), responseBody, requestUrl);

                return Ok(offers);
            }
            catch (Exception ex)
            {
                _logger.LogError(LogMessages.GetOfferDataError + ex.Message);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> NewOffer(OfferWithPicModel offer)
        {
            try
            {
                if(offer.Name == null || offer.ValidFrom == null || offer.Price == null)
                {
                    return BadRequest(DisplayMessages.NullInput);
                }
                else
                {
                    Offer o = new Offer
                    {
                        Name = offer.Name,
                        Description = offer.Description,
                        Price = offer.Price,
                        DiscountPercentage = offer.Discount,
                        ValidFrom = offer.ValidFrom,
                        ValidTo = offer.ValidTo,
                        IsRoomOffer = offer.IsRoomOffer,
                        CreatedBy = offer.CreatedBy
                    };

                    int offerId = await _offerManager.NewOffer(o);

                    if(offerId > 0)
                    {
                        if (offer?.offerPics?.Length > 0)
                        {
                            PicDocument pic = new PicDocument
                            {
                                PicType = PicType.Offer,
                                PicTypeId = offerId,
                                CreatedBy = offer.CreatedBy
                            };

                            foreach (var op in offer.offerPics)
                            {
                                pic.PicValue = op.PicValue;
                                pic.PicName = op.PicName;
                                pic.CloudinaryPublicId = op.CloudinaryPublicId;

                                _documentManager.UploadImage(pic);
                            }
                        }

                        string requestUrl = HttpContext.Request.Path.ToString();
                        string responseBody = JsonConvert.SerializeObject(offer);

                        _log.setLogTrace(new HttpRequestMessage(), new HttpResponseMessage(), requestUrl, responseBody);
                        return Ok(offer);
                    }
                    else
                    {
                        return StatusCode(500, "Internal Server Error");
                    }

                }

            }
            catch (Exception ex)
            {
                _logger.LogError(LogMessages.InsertDataError + ex.Message);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPatch]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangeActiveStatus(Offer o)
        {
            try
            {
                if (o.OfferId > 0)
                {
                    bool isStatusChanged = false;
                    Offer offer = await _offerManager.GetOfferById(o.OfferId);

                    if (offer.Name != null)
                    {
                        isStatusChanged = await _offerManager.ChangeActiveStatus(o);

                        if (isStatusChanged)
                        {
                            return Ok(true);
                        }
                        else
                        {
                            return BadRequest(DisplayMessages.StatusChangeError);
                        }
                    }
                    else
                    {
                        return BadRequest(DisplayMessages.EmptyExistData + o.OfferId);
                    }
                }
                else
                {
                    return BadRequest(DisplayMessages.InvalidId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(LogMessages.StatusChangeError + ex.Message);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet]
        [Route("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> FindOfferById([FromRoute] int Id)
        {
            try
            {
                if(Id > 0)
                {
                    Offer offer = await _offerManager.GetOfferById(Id);

                    string responseBody = JsonConvert.SerializeObject(offer);

                    string requestUrl = HttpContext.Request.Path.ToString();

                    _log.setLogTrace(new HttpRequestMessage(), new HttpResponseMessage(), responseBody, requestUrl);

                    if(offer.Name != null)
                    {
                        return Ok(offer);
                    }
                    else
                    {
                        return NotFound(DisplayMessages.EmptyExistData);
                    }
                }
                else
                {
                    return NotFound(DisplayMessages.InvalidId);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(LogMessages.FindDataByIdError + Id + " : " + ex.Message);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPut]
        [Route("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateOffer(OfferWithPicModel offer, [FromRoute] int Id)
        {
            try
            {
                bool isNameExist = false;
                bool isNameChange = false;

                if (Id > 0)
                {
                    Offer oldOffer = await _offerManager.GetOfferById(Id);
                    if (oldOffer.Name != null)
                    {
                        Offer updatedOffer = new Offer
                        {
                            Name = offer.Name,
                            Description = offer.Description,
                            ValidFrom = offer.ValidFrom,
                            ValidTo = offer.ValidTo,
                            Price = offer.Price,
                            DiscountPercentage = offer.Discount,
                            IsActive = offer.IsActive,
                            IsRoomOffer = offer.IsRoomOffer,
                            UpdatedBy = offer.UpdatedBy
                        };
                        _offerManager.UpdateOffer(updatedOffer, Id);


                        if (offer?.offerPics?.Length > 0)
                        {
                            PicDocument pic = new PicDocument
                            {
                                PicType = PicType.Offer,
                                PicTypeId = Id,
                                CreatedBy = offer.UpdatedBy
                            };

                            foreach (var item in offer.offerPics)
                            {
                                pic.PicValue = item.PicValue;
                                pic.PicName = item.PicName;
                                pic.CloudinaryPublicId = item.CloudinaryPublicId;

                                _documentManager.UploadImage(pic);
                            }
                        }

                        string requestUrl = HttpContext.Request.Path.ToString();
                        string responseBody = JsonConvert.SerializeObject(offer);

                        _log.setLogTrace(new HttpRequestMessage(), new HttpResponseMessage(), requestUrl, responseBody);
                        return Ok(offer);
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
                _logger.LogError(LogMessages.UpdateDataError + ex.Message);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpDelete]
        [Route("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteOffer([FromRoute] int Id)
        {
            try
            {
                if(Id > 0)
                {
                    bool isOfferRemove = false;
                    List<string> publicIds = new List<string>();
                    bool IsRemoveFromCLoudinary = false;

                    Offer offer = await _offerManager.GetOfferById(Id);

                    if (offer.Name != null)
                    {
                        if (offer?.OfferPics?.Count > 0)
                        {
                            foreach (var pic in offer.OfferPics)
                            {
                                publicIds.Add(pic.CloudinaryPublicId ?? "");
                            }
                            IsRemoveFromCLoudinary = _documentManager.DeleteAssetFromCloudinary(publicIds).Result;

                            if (IsRemoveFromCLoudinary)
                            {
                                isOfferRemove = await _offerManager.DeleteOffer(Id);
                            }
                        }
                        else
                        {
                            isOfferRemove = await _offerManager.DeleteOffer(Id);
                        }
                        

                        string requestUrl = HttpContext.Request.Path.ToString();
                        string responseBody = JsonConvert.SerializeObject(offer);

                        _log.setLogTrace(new HttpRequestMessage(), new HttpResponseMessage(), requestUrl, responseBody);

                        if (isOfferRemove)
                        {
                            return Ok(offer);
                        }
                        else
                        {
                            return BadRequest(DisplayMessages.DeletingError);
                        }
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
                _logger.LogError(LogMessages.DeleteOfferError + ex.Message);
                return StatusCode(500, "Internal Server Error");
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
                    return BadRequest(DisplayMessages.NullInput);
                }
                else
                {
                    List<string> idArray = new List<string>();
                    idArray = ids.Split(',').ToList();
                    bool IsRemoveFromCLoudinary = false;
                    bool isDeleted = false;

                    IsRemoveFromCLoudinary = _documentManager.DeleteAssetFromCloudinary(idArray).Result;

                    if (IsRemoveFromCLoudinary)
                    {
                        int picType = 3;
                        isDeleted = _documentManager.DeleteImageFromDB(idArray, picType).Result;

                        if (!isDeleted)
                        {
                            return BadRequest(DisplayMessages.ImageDeleteError);
                        }
                        else
                        {
                            string requestUrl = HttpContext.Request.Path.ToString();
                            string responseBody = JsonConvert.SerializeObject(ids);

                            _log.setLogTrace(new HttpRequestMessage(), new HttpResponseMessage(), requestUrl, responseBody);
                            return Ok("Deleted " + responseBody);
                        }
                    }
                    else
                    {
                        return BadRequest(DisplayMessages.CloudinaryError);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(LogMessages.DeleteImageError + ex.Message);
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
