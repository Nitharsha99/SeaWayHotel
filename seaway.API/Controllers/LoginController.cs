using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions;
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
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;

namespace seaway.API.Controllers
{
    [Route("api/admin")]
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
        //[Route("")]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public IActionResult NewAdmin([FromForm]Admin admin)
        //{
        //    try
        //    {
        //        if(admin == null)
        //        {
        //            return BadRequest("Admin details are empty");
        //        }
        //        else
        //        {
        //            if(admin.Password != null)
        //            {
        //                admin.Password = PasswordHelper.EncryptPassword(admin.Password);
        //            }

        //            var newAdmin = _logginManager.NewAdmin(admin);

        //            string responseBody = JsonConvert.SerializeObject(admin);
        //            string requestUrl = HttpContext.Request.Path.ToString();

        //            _log.setLogTrace(new HttpRequestMessage(), new HttpResponseMessage(), responseBody, requestUrl);

        //            if (newAdmin == null)
        //            {
        //                return BadRequest("Creation of new admin Failed");
        //            }
        //            else
        //            {
        //                return Ok(newAdmin);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError("An exception occurred while creating new admin : " + ex.Message);
        //        return BadRequest(ex.Message);
        //    }
        //}

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Login(Admin admin)
        {
            try
            {
                IActionResult response = Unauthorized();
                var _admin = AuthenticateUser(admin);

                if (_admin != null)
                {
                    var token = GenerateToken();
                    response = Ok(new { token = token });
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("An exception occurred while login : " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        private Admin AuthenticateUser(Admin admin)
        {
            Admin _admin = null;
            if (admin.IsAdmin == true)
            {
                if(admin.Username == "Nitharsha" && admin.Password == "12345")
                {
                    _admin = new Admin
                    {
                        Username = "mmmm"
                    };
                }
                return _admin;
            }
            else
            {
                return _admin ?? new Admin();
            }
        }

        private string GenerateToken()
        {
            var keyBytes = Encoding.UTF8.GetBytes(_configuration["JWTSettings:Key"]);
            var securityKey = new SymmetricSecurityKey(keyBytes);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_configuration["JWTSettings:Issuer"], _configuration["JWTSettings:Audience"], null, 
                expires: DateTime.Now.AddMinutes(360), signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}


