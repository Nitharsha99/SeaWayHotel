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

//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Web;

//namespace Optimo.WebAPI.Handlers
//{
//    public class CustomLogHandler : DelegatingHandler
//    {

 
//        private LogMetadata BuildRequestMetadata(HttpRequestMessage request)
//        {
//            try
//            {
//                LogMetadata log = new LogMetadata
//                {
//                    RequestMethod = request.Method.Method,
//                    RequestTimestamp = DateTime.Now,
//                    RequestUri = request.RequestUri.ToString(),
//                    RequestBody = request.Content != null ? request.Content.ReadAsStringAsync().Result : ""
//                };
//                return log;
//            }
//            catch (Exception e)
//            {
//                return new LogMetadata
//                {
//                    RequestMethod = request.Method.Method,
//                    RequestTimestamp = DateTime.Now,
//                    RequestUri = request.RequestUri.ToString(),
//                    RequestBody = e.Message + " " + e.StackTrace
//                };
//            }
//        }
//        private LogMetadata BuildResponseMetadata(LogMetadata logMetadata, HttpResponseMessage response)
//        {
//            if (response != null)
//            {
//                logMetadata.ResponseStatusCode = response.StatusCode;
//                logMetadata.ResponseTimestamp = DateTime.Now;
//                //logMetadata.ResponseContentType = response.Content.Headers.ContentType.MediaType;
//                if (response.Content != null)
//                    logMetadata.Content = response.Content.ReadAsStringAsync().Result;
//            }
//            return logMetadata;
//        }
//        private async Task<bool> SendToLog(LogMetadata logMetadata)
//        {
//            // TODO: Write code here to store the logMetadata instance to a pre-configured log store...
//            var logger = NLog.LogManager.GetCurrentClassLogger();
//            try
//            {

//                logger.Trace("************************ ");
//                logger.Trace("Request: " + logMetadata.RequestMethod + " : " + logMetadata.RequestUri);
//                logger.Trace("Request Body: " + logMetadata.RequestBody);
//                logger.Trace("RequestTimestamp: " + logMetadata.RequestTimestamp.Value.ToString("dd-MMM-yyyy HH:mm:ss"));
//                logger.Trace("ResponseTimestamp: " + logMetadata.ResponseTimestamp.Value.ToString("dd-MMM-yyyy HH:mm:ss"));
//                logger.Trace("Response Content: " + logMetadata.Content);
//                logger.Trace("************************ ");
//            }
//            catch (Exception e)
//            {
//                logger.Error(e, "SendToLog");
//            }
//            return true;
//        }
//    }
//    public class LogMetadata
//    {
//        public string RequestContentType { get; set; }
//        public string RequestUri { get; set; }
//        public string RequestMethod { get; set; }
//        public string RequestBody { get; set; }
//        public DateTime? RequestTimestamp { get; set; }
//        public string ResponseContentType { get; set; }
//        public HttpStatusCode ResponseStatusCode { get; set; }
//        public DateTime? ResponseTimestamp { get; set; }
//        public string Content { get; set; }
//    }

//}

