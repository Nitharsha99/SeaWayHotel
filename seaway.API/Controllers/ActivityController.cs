using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using seaway.API.Configurations;
using seaway.API.Manager;
using seaway.API.Models;
using seaway.API.Models.ViewModels;
using System.Data.SqlClient;
using System.IO;

namespace seaway.API.Controllers
{
    [Route("api/Activity")]
    [ApiController]
    public class ActivityController : ControllerBase
    {

        //private readonly ILogger<LogHandler> _logger;
        private readonly IConfiguration _configuration;
        public readonly IWebHostEnvironment _environment;
        //LogHandler _log;
        //ActivityManager _activityManager;
        PicDocumentManager _documentManager;

        public ActivityController( IConfiguration configuration,   IWebHostEnvironment environment, PicDocumentManager documentManager)
        {
            //_logger = logger;
            _configuration = configuration;
            _environment = environment;
            //_log = log;
            //_activityManager = activityManager;
            _documentManager = documentManager;
        }

        [HttpPost]
        public async Task<IActionResult> PostActivity(ActivityWithPicModel activity)
        {
            try
            {
                Activity act= new Activity();
                PicDocument pic= new PicDocument();



              
                return Ok(activity);
            }
            catch (Exception ex)
            {
                //_logger.LogError("An exception occurred while retrieving admins : " + ex.Message);
                return BadRequest(ex.Message);
            }

        }

        //[HttpPost]
        //public IActionResult PostActivities() 
        //{
        //    try
        //    {
        //        return;
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest();
        //    }
        //} 

    }
}
