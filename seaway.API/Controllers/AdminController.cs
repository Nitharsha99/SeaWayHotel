using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using seaway.API.Configurations;
using seaway.API.Manager;
using seaway.API.Models;

namespace seaway.API.Controllers
{
    [Route("api/Admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ILogger<LogHandler> _logger;
        private readonly IConfiguration _configuration;
        private readonly AdminManager _adminManager;
        LogHandler _log;

        public AdminController(ILogger<LogHandler> logger, IConfiguration configuration, LogHandler log, AdminManager adminManager)
        {
            _logger = logger;
            _configuration = configuration;
            _log = log;
            _adminManager = adminManager;
        }

        [HttpPost]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult NewAdmin(Admin admin)
        {
            try
            {
                if (admin == null)
                {
                    return BadRequest("Admin details are empty");
                }
                else
                {
                    if (admin.Password != null)
                    {
                        admin.Password = PasswordHelper.EncryptPassword(admin.Password);
                    }

                    var newAdmin = _logginManager.NewAdmin(admin);

                    string responseBody = JsonConvert.SerializeObject(admin);
                    string requestUrl = HttpContext.Request.Path.ToString();

                    _log.setLogTrace(new HttpRequestMessage(), new HttpResponseMessage(), responseBody, requestUrl);

                    if (newAdmin == null)
                    {
                        return BadRequest("Creation of new admin Failed");
                    }
                    else
                    {
                        return Ok(newAdmin);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("An exception occurred while creating new admin : " + ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
