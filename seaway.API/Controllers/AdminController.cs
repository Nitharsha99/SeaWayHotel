﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using seaway.API.Configurations;
using seaway.API.Manager;
using seaway.API.Models;
using seaway.API.Models.ViewModels;

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
        public async Task<IActionResult> NewAdmin([FromForm]AdminWithProfilePic admin)
        {
            try
            {
                string filePath = null;
                if (admin == null)
                {
                    return BadRequest(DisplayMessages.NullInput);
                }
                else
                {
                    if(admin.ProfilePic != null)
                    {
                        var folder = Path.Combine("wwwroot", "Uploads");
                        if (!Directory.Exists(folder))
                        {
                            Directory.CreateDirectory(folder);
                        }
                        filePath = Path.Combine(Directory.GetCurrentDirectory(), folder, admin.ProfilePic.FileName);

                        using(var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await admin.ProfilePic.CopyToAsync(stream);
                        }
                    }

                    var newAdmin = new Admin
                    {
                        Username = admin.Username,
                        Password = admin.Password,
                        IsAdmin = admin.IsAdmin,
                        ProfilePicPath = filePath,
                        CreatedBy = admin.CreatedBy
                    };

                    bool isCreated = _adminManager.NewAdmin(newAdmin);

                    string responseBody = JsonConvert.SerializeObject(admin);
                    string requestUrl = HttpContext.Request.Path.ToString();

                    _log.setLogTrace(new HttpRequestMessage(), new HttpResponseMessage(), responseBody, requestUrl);

                    if (!isCreated)
                    {
                        return BadRequest(DisplayMessages.InsertingError);
                    }
                    else
                    {
                        return Ok(newAdmin);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(LogMessages.InsertDataError + ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
