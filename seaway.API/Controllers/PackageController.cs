using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using seaway.API.Configurations;
using seaway.API.Manager;
using seaway.API.Models;

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
    }
}
