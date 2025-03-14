using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using seaway.API.Configurations;
using seaway.API.Manager;
using seaway.API.Models;
using System.Net;
using System.Text;


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

        [Route("{packageId:int}")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPackageById(int packageId)
        {
            try
            {
                if (packageId <= 0)
                {
                    return BadRequest("Invalid package ID.");
                }
                Package package = await _packageManager.GetPackageById(packageId);
                if (package == null)
                {
                    _logger.LogWarning($"Package with ID {packageId} not found");
                    return NotFound($"Package with ID {packageId} does not exist.");
                }
                string responseBody = JsonConvert.SerializeObject(package);
                string requestUrl = HttpContext.Request.Path.ToString();
                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(responseBody, Encoding.UTF8, "application/json")
                };

                _log.setLogTrace(new HttpRequestMessage(), new HttpResponseMessage(), responseBody, requestUrl);

                return Ok(package);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching package data:{ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpDelete("{packageId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletePackage(int packageId)
        {
            try
            {
                if (packageId <= 0)
                {
                    return BadRequest("Invalid package id");
                }
                bool isDeleted = await _packageManager.DeletePackage(packageId);
                if (!isDeleted)
                {
                    _logger.LogWarning($"Package with Id {packageId} not found.");
                    return NotFound($"Package with Id {packageId} does not exist.");
                }
                return Ok($"Package with Id {packageId} deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting package: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
