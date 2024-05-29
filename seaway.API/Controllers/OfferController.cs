using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using seaway.API.Configurations;
using seaway.API.Manager;
using seaway.API.Models;

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

        public OfferController(ILogger<LogHandler> logger, IConfiguration configuration, LogHandler log, OfferManager offerManager)
        {
            _configuration = configuration;
            _logger = logger;
            _log = log;
            _offerManager = offerManager;
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

                    if (offer.OfferName != null)
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
    }
}
