using Microsoft.AspNetCore.Mvc;
using seaway.API.Models;
using seaway.API.Manager;
using seaway.API.Configurations;
using seaway.API.Models.ViewModels;
using Newtonsoft.Json;
using MailKit;

namespace seaway.API.Controllers
{
    [Route("api/Login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LogHandler> _logger;
        private readonly IConfiguration _configuration;
        private readonly EmailManager _emailManager;
        LoginManager _logginManager;
        LogHandler _log;

        public LoginController(ILogger<LogHandler> logger, IConfiguration configuration, LoginManager loginManager, EmailManager emailManager)
        {
            _logger = logger;
            _configuration = configuration;
            _logginManager = loginManager;
            _log = new LogHandler(logger);
            _emailManager = emailManager;
        }

        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login(LoginModel login)
        {
            try
            {
                if (login.Username == null)
                {
                    return BadRequest(DisplayMessages.NullInput);
                }
                else
                {
                    bool isValidUser = false;
     
                    isValidUser = await _logginManager.CheckUserValid(login);

                    string responseBody = JsonConvert.SerializeObject(login);
                    string requestUrl = HttpContext.Request.Path.ToString();

                     _log.setLogTrace(new HttpRequestMessage(), new HttpResponseMessage(), responseBody, requestUrl);

                    return Ok(isValidUser);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(LogMessages.LoginError + ex.Message);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost]
        [Route("SendMail")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SendMail(Email mail)
        {
            try
            {
                await _emailManager.SendMail(mail);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(LogMessages.LoginError + ex.Message);
                return StatusCode(500, "Internal Server Error");
            }
        }

    }
}


