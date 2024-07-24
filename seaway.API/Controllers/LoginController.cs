using Microsoft.AspNetCore.Mvc;
using seaway.API.Models;
using seaway.API.Manager;
using seaway.API.Configurations;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;

namespace seaway.API.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        //private readonly ILogger<LogHandler> _logger;
        //private readonly IConfiguration _configuration;
        //LoginManager _logginManager;
        //LogHandler _log;

        //public LoginController(ILogger<LogHandler> logger, IConfiguration configuration)
        //{
        //    _logger = logger;
        //    _configuration = configuration;
        //    _logginManager = new LoginManager(logger, configuration);
        //    _log = new LogHandler(logger);
        //}

        //[Route("")]
        //[HttpGet]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public async Task<IActionResult> GetAdmins()
        //{
        //    try
        //    {
        //        List<Admin> adminList = _logginManager.GetAdmin();

        //        string responseBody = JsonConvert.SerializeObject(adminList);

        //        string requestUrl = HttpContext.Request.Path.ToString();

        //        _log.setLogTrace(new HttpRequestMessage(), new HttpResponseMessage(), responseBody, requestUrl);


        //        return Ok(adminList);
        //    }
        //    catch(Exception ex)
        //    {
        //        _logger.LogError("An exception occurred while retrieving admins : " + ex.Message);
        //        return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
        //    }
        //}

        //[HttpPost]
        //[Route("Login")]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public IActionResult Login(Admin admin)
        //{
        //    try
        //    {
        //        if( admin == null)
        //        {
        //            return BadRequest("login credentials are empty");
        //        }
        //        else
        //        {
        //            bool isValidUser = false;
        //            if(admin.Password != null)
        //            {
        //                isValidUser = _logginManager.CheckUserValid(admin);
        //            }
        //            return Ok(isValidUser);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError("An exception occurred while login : " + ex.Message);
        //        return BadRequest(ex.Message);
        //    }
        //}

    
    }
}


