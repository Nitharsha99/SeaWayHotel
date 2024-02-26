using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using seaway.API.Models;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json.Linq;
using System.Net;
using seaway.API.Manager;
using Newtonsoft.Json;
using seaway.API.Configurations;
using System.Net.Http;

namespace seaway.API.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LogHandler> _logger;
        private readonly IConfiguration _configuration;
        LoginManager _logginManager;
        LogHandler _log;

        public LoginController(ILogger<LogHandler> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _logginManager = new LoginManager(logger, configuration);
            _log = new LogHandler(logger);
        }

        [Route("")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAdmins()
        {
            try
            {
                List<Admin> adminList = _logginManager.GetAdmin();

                string responseBody = JsonConvert.SerializeObject(adminList);

                string requestUrl = HttpContext.Request.Path.ToString();

                _log.setLogTrace(new HttpRequestMessage(), new HttpResponseMessage(), responseBody, requestUrl);


                return Ok(adminList);
            }
            catch(Exception ex)
            {
                _logger.LogError("An exception occurred while retrieving admins : " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

    }
}


