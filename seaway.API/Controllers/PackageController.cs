using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using seaway.API.Configurations;
using seaway.API.Manager;
using seaway.API.Models;
using seaway.API.Models.Enum;
using seaway.API.Models.ViewModels;
using System.Diagnostics;
using System;

namespace seaway.API.Controllers
{
    [Route("api/Package")]
    [ApiController]
    public class PackageController : ControllerBase
    {
        private readonly ILogger<LogHandler> _logger;
        private readonly IConfiguration _configuration;
        public readonly IWebHostEnvironment _environment;
        LogHandler _log;
        PackageManager _packageManager;
        private readonly PicDocumentManager _documentManager;

        public PackageController(ILogger<LogHandler> logger, IConfiguration configuration, IWebHostEnvironment environment, LogHandler log, PackageManager packageManager, PicDocumentManager documentManager)
        {
            _logger = logger;
            _configuration = configuration;
            _environment = environment;
            _log = log;
            _packageManager = packageManager;
            _documentManager = documentManager;
        }

        [Route("")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllPackages()
        {
            try
            {
                List<Package> packages = await _packageManager.GetAllPackages();

                string responseBody = JsonConvert.SerializeObject(packages);

                string requestUrl = HttpContext.Request.Path.ToString();

                _log.setLogTrace(new HttpRequestMessage(), new HttpResponseMessage(), responseBody, requestUrl);

                return Ok(packages);
            }
            catch (Exception ex)
            {
                _logger.LogError(LogMessages.GetPackageDataError + ex.Message);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> NewPackage([FromForm] PackageWithPicModel package)
        {
            try
            {
                bool isNameExist = false;
                if (package.Name == null || package.DurationType == 0 || package.UserType == 0)
                {
                    return BadRequest(DisplayMessages.NullInput);
                }
                else
                {
                    isNameExist = await _packageManager.IsNameExist(package.Name);

                    if (isNameExist)
                    {
                        return BadRequest(DisplayMessages.ExistNameError);
                    }
                    else
                    {
                        Package pack = new Package
                        {
                            Name = package.Name,
                            Description = package.Description,
                            DurationType = package.DurationType,
                            Price = package.Price,
                            UserType = package.UserType,
                            CreatedBy = package?.CreatedBy,
                        };

                        int packId = await _packageManager.NewPackage(pack);

                        if (packId > 0)
                        {
                            if (package?.packagePics?.Length > 0)
                            {
                                PicDocument pic = new PicDocument
                                {
                                    PicType = PicType.Package,
                                    PicTypeId = packId,
                                    CreatedBy = package.CreatedBy
                                };

                                foreach (var item in package.packagePics)
                                {
                                    pic.PicValue = item.PicValue;
                                    pic.PicName = item.PicName;
                                    pic.CloudinaryPublicId = item.CloudinaryPublicId;

                                    _documentManager.UploadImage(pic);
                                }
                            }

                            string requestUrl = HttpContext.Request.Path.ToString();
                            string responseBody = JsonConvert.SerializeObject(package);

                            _log.setLogTrace(new HttpRequestMessage(), new HttpResponseMessage(), requestUrl, responseBody);
                            return Ok(package);
                        }
                        else
                        {
                            return StatusCode(500, "Internal Server Error");
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(LogMessages.InsertDataError + ex.Message);
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
