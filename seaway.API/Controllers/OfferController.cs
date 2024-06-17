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
                _logger.LogError("An exception occurred while inserting offer data : " + ex.Message);
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
                _logger.LogError("An exception occurred while changing Active status of Offer : " + ex.Message);
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
                _logger.LogError("An exception occurred while get offer data with Id = " + Id + " : " + ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
