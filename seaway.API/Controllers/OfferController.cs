using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using seaway.API.Configurations;
using seaway.API.Manager;
using seaway.API.Models;
using seaway.API.Models.ViewModels;

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
        public IActionResult GetAllOffers()
        {
            try
            {
                List<Offer> offers = _offerManager.GetOffers();

                string responseBody = JsonConvert.SerializeObject(offers);

                string requestUrl = HttpContext.Request.Path.ToString();

                _log.setLogTrace(new HttpRequestMessage(), new HttpResponseMessage(), responseBody, requestUrl);

                return Ok(offers);
            }
            catch (Exception ex)
            {
                _logger.LogError(LogMessages.GetOfferDataError + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult NewOffer(OfferWithPicModel offer)
        {
            try
            {
                if(offer.Name == null || offer.ValidFrom == null || offer.Price == null)
                {
                    return BadRequest("There is no any new data of Offer");
                }
                else
                {
                    Offer o = new Offer
                    {
                        Name = offer.Name,
                        Description = offer.Description,
                        Price = offer.Price,
                        DiscountPercentage = offer.DiscountPercentage,
                        ValidFrom = offer.ValidFrom,
                        ValidTo = offer.ValidTo,
                        IsRoomOffer = offer.IsRoomOffer
                    };

                    int offerId = _offerManager.NewOffer(o);

                    PicDocument pic = new PicDocument
                    {
                        PicType = "Offer",
                        PicTypeId = offerId
                    };

                    if(offer?.offerPics?.Length > 0)
                    {
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
                }

                return Ok(offer);
            }
            catch (Exception ex)
            {
                _logger.LogError(LogMessages.InsertDataError + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ChangeActiveStatus(bool status, int id)
        {
            try
            {
                if (id != 0)
                {
                    bool isStatusChanged = false;
                    Offer offer = _offerManager.GetOfferById(id);

                    if (offer.Name != null)
                    {
                        isStatusChanged = _offerManager.ChangeActiveStatus(status, id);

                        if (isStatusChanged)
                        {
                            return Ok(true);
                        }
                        else
                        {
                            return BadRequest("Error on changing offer status");
                        }
                    }
                    else
                    {
                        return BadRequest("There is no any offer with this Id --> " + id);
                    }
                }
                return BadRequest("Id is not Supplied");
            }
            catch (Exception ex)
            {
                _logger.LogError(LogMessages.StatusChangeError + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult FindOfferById([FromRoute] int Id)
        {
            try
            {
                Offer offer = _offerManager.GetOfferById(Id);

                string responseBody = JsonConvert.SerializeObject(offer);

                string requestUrl = HttpContext.Request.Path.ToString();

                _log.setLogTrace(new HttpRequestMessage(), new HttpResponseMessage(), responseBody, requestUrl);
                return Ok(offer);
            }
            catch (Exception ex)
            {
                _logger.LogError(LogMessages.FindOfferByIdError + Id + " : " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateOffer(OfferWithPicModel offer, [FromRoute] int Id)
        {
            try
            {
                if (Id != 0)
                {
                    Offer oldOffer = _offerManager.GetOfferById(Id);
                    if (oldOffer.Name != null)
                    {
                        Offer updatedOffer = new Offer
                        {
                            Name = offer.Name,
                            Description = offer.Description,
                            ValidFrom = offer.ValidFrom,
                            ValidTo = offer.ValidTo,
                            Price = offer.Price,
                            DiscountPercentage = offer.DiscountPercentage,
                            IsActive = offer.IsActive,
                            IsRoomOffer = offer.IsRoomOffer
                        };
                        _offerManager.UpdateOffer(updatedOffer, Id);

                        PicDocument pic = new PicDocument
                        {
                            PicType = "Offer",
                            PicTypeId = Id
                        };

                        if (offer?.offerPics?.Length > 0)
                        {
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

                    }
                    else
                    {
                        return BadRequest("Invalid OfferId");
                    }
                }

                return Ok(offer);
            }
            catch (Exception ex)
            {
                _logger.LogError(LogMessages.UpdateDataError + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteOffer([FromRoute] int Id)
        {
            try
            {
                bool isOfferRemove = false;
                List<string> publicIds = new List<string>();
                bool IsRemoveFromCLoudinary = false;

                Offer offer = _offerManager.GetOfferById(Id);

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
                            isOfferRemove = _offerManager.DeleteOffer(Id);
                        }
                    }
                    isOfferRemove = _offerManager.DeleteOffer(Id);

                    string requestUrl = HttpContext.Request.Path.ToString();
                    string responseBody = JsonConvert.SerializeObject(offer);

                    _log.setLogTrace(new HttpRequestMessage(), new HttpResponseMessage(), requestUrl, responseBody);

                    if (isOfferRemove)
                    {
                        return Ok(offer);
                    }
                    else
                    {
                        return BadRequest("Issue in deleting process");
                    }
                }
                else
                {
                    return BadRequest("Offer is not exist for this Id");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(LogMessages.DeleteOfferError + ex.Message);
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

                IsRemoveFromCLoudinary = _documentManager.DeleteAssetFromCloudinary(idArray).Result;

                if (IsRemoveFromCLoudinary)
                {
                    string picType = "Offer";
                    _documentManager.DeleteImageFromDB(idArray, picType);
                }

                string requestUrl = HttpContext.Request.Path.ToString();
                string responseBody = JsonConvert.SerializeObject(ids);

                _log.setLogTrace(new HttpRequestMessage(), new HttpResponseMessage(), requestUrl, responseBody);
                return Ok("Deleted " + responseBody);
            }
            catch (Exception ex)
            {
                _logger.LogError(LogMessages.DeleteImageError + ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
