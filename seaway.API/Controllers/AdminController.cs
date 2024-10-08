using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using seaway.API.Configurations;
using seaway.API.Manager;
using seaway.API.Models;
using seaway.API.Models.ViewModels;
using System.Diagnostics;

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
                string? filePath = null;
                bool isNameExist = false;
                if (admin.Username == null)
                {
                    return BadRequest(DisplayMessages.NullInput);
                }
                else
                {
                    isNameExist = await _adminManager.IsUsernameExist(admin.Username);

                    if (isNameExist)
                    {
                        return BadRequest(DisplayMessages.ExistNameError);
                    }
                    else
                    {
                        //if (admin.ProfilePic != null)
                        //{
                        //    var currentDir = Directory.GetCurrentDirectory();
                        //    var folder = Path.Combine(currentDir, "wwwroot", "Uploads");
                        //    if (!Directory.Exists(folder))
                        //    {
                        //        Directory.CreateDirectory(folder);
                        //    }
                        //    filePath = Path.Combine(Directory.GetCurrentDirectory(), folder, admin.ProfilePic.FileName);

                        //    using (var stream = new FileStream(filePath, FileMode.Create))
                        //    {
                        //        await admin.ProfilePic.CopyToAsync(stream);
                        //    }
                        //}

                        var newAdmin = new Admin
                        {
                            Username = admin.Username,
                            Password = admin.Password,
                            IsAdmin = admin.IsAdmin,
                            PicPath = admin.PicPath,
                            PicName = admin.PicName,
                            PublicId = admin.PublicId,
                            CreatedBy = admin.CreatedBy
                        };

                        bool isCreated = await _adminManager.NewAdmin(newAdmin);

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
            }
            catch (Exception ex)
            {
                _logger.LogError(LogMessages.InsertDataError + ex.Message);
                return StatusCode(500, "Internal Server Error");  
            }
        }

        [Route("")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllAdmins()
        {
            try
            {
                List<Admin> admins = await _adminManager.GetAllAdmins();

                string responseBody = JsonConvert.SerializeObject(admins);

                string requestUrl = HttpContext.Request.Path.ToString();

                _log.setLogTrace(new HttpRequestMessage(), new HttpResponseMessage(), responseBody, requestUrl);

                return Ok(admins);
            }
            catch(Exception ex)
            {
                _logger.LogError(LogMessages.GetAdminDataError + ex.Message);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet]
        [Route("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAdminById([FromRoute] int Id)
        {
            try
            {
                if(Id > 0)
                {
                    Admin admin = await _adminManager.GetAdminById(Id);

                    string responseBody = JsonConvert.SerializeObject(admin);

                    string requestUrl = HttpContext.Request.Path.ToString();

                    _log.setLogTrace(new HttpRequestMessage(), new HttpResponseMessage(), responseBody, requestUrl);

                    if (admin.Username != null)
                    {
                        return Ok(admin);
                    }
                    else
                    {
                        return NotFound(DisplayMessages.EmptyExistData + Id);
                    }
                }
                else
                {
                    return BadRequest(DisplayMessages.InvalidId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(LogMessages.FindDataByIdError + Id + " : " + ex.Message);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPut]
        [Route("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateAdmin([FromForm]AdminWithProfilePic admin, int Id)
        {
            try
            {
                if(Id > 0)
                {
                    bool isNameExist = false;
                    bool isNameChange = false;
                    string? filePath = null;

                    Admin existAdmin = await _adminManager.GetAdminById(Id);
                    if (existAdmin.Username != null)
                    {
                        if (admin.Username != null)
                        {
                            isNameChange = _adminManager.IsNameChange(admin.Username, existAdmin.Username);

                            if (isNameChange)
                            {
                                isNameExist =   await _adminManager.IsUsernameExist(admin.Username);
                            }
                        }
                        if (isNameExist)
                        {
                            return BadRequest(DisplayMessages.ExistNameError);
                        }
                        else
                        {
                            Admin updateAdmin = new Admin
                            {
                                AdminId = Id,
                                Username = admin.Username,
                                IsAdmin = admin.IsAdmin,
                                PublicId = admin.PublicId,
                                PicName = admin.PicName,
                                PicPath = admin.PicPath,
                                UpdatedBy = admin.UpdatedBy
                            };

                            _adminManager.UpdateAdmin(updateAdmin);

                            string requestUrl = HttpContext.Request.Path.ToString();
                            string responseBody = JsonConvert.SerializeObject(updateAdmin);

                            _log.setLogTrace(new HttpRequestMessage(), new HttpResponseMessage(), requestUrl, responseBody);

                            return Ok(updateAdmin);
                        }
                    }
                    else
                    {
                        return NotFound(DisplayMessages.EmptyExistData + Id);
                    }
                }
                else
                {
                    return BadRequest(DisplayMessages.InvalidId);
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(LogMessages.UpdateDataError + ex.Message);
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
