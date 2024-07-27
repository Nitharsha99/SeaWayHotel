using Microsoft.AspNetCore.Mvc;
using seaway.API.Models;
using seaway.API.Manager;
using seaway.API.Configurations;
using seaway.API.Models.ViewModels;
using Newtonsoft.Json;

namespace seaway.API.Controllers
{
    [Route("api/Login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LogHandler> _logger;
        private readonly IConfiguration _configuration;
        LoginManager _logginManager;
        LogHandler _log;

        public LoginController(ILogger<LogHandler> logger, IConfiguration configuration, LoginManager loginManager)
        {
            _logger = logger;
            _configuration = configuration;
            _logginManager = loginManager;
            _log = new LogHandler(logger);
        }

        [HttpPost]
        [Route("Login")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Login(LoginModel login)
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
     
                    isValidUser = _logginManager.CheckUserValid(login);

                    string responseBody = JsonConvert.SerializeObject(login);
                    string requestUrl = HttpContext.Request.Path.ToString();

                     _log.setLogTrace(new HttpRequestMessage(), new HttpResponseMessage(), responseBody, requestUrl);

                    return Ok(isValidUser);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(LogMessages.LoginError + ex.Message);
                return BadRequest(ex.Message);
            }
        }


    }
}


