using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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

        private readonly ILogger<LogHandler> _logger;
        private readonly IConfiguration _configuration;
        public readonly IWebHostEnvironment _environment;
        LogHandler _log;
        ActivityManager _activityManager;
        private readonly PicDocumentManager _documentManager;

        public ActivityController( IConfiguration configuration,   IWebHostEnvironment environment, PicDocumentManager documentManager, ActivityManager activityManager, LogHandler log, ILogger<LogHandler> logger)
        {
            _logger = logger;
            _configuration = configuration;
            _environment = environment;
            _log = log;
            _activityManager = activityManager;
            _documentManager = documentManager;
        }

        [Route("")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetAllActivities()
        {
            try
            {
                List<Activity> activities = _activityManager.GetActivities();

                string responseBody = JsonConvert.SerializeObject(activities);

                string requestUrl = HttpContext.Request.Path.ToString();

                _log.setLogTrace(new HttpRequestMessage(), new HttpResponseMessage(), responseBody, requestUrl);

                return Ok(activities);
            }
            catch (Exception ex)
            {
                _logger.LogError("An exception occurred while get all activities data : " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult FindRoomById([FromRoute] int Id)
        {
            try
            {
                Activity activity = _activityManager.GetActivityById(Id);

                string responseBody = JsonConvert.SerializeObject(activity);

                string requestUrl = HttpContext.Request.Path.ToString();

                _log.setLogTrace(new HttpRequestMessage(), new HttpResponseMessage(), responseBody, requestUrl);
                return Ok(activity);
            }
            catch (Exception ex)
            {
                _logger.LogError("An exception occurred while get activity data with Id = " + Id + " : " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        //[HttpDelete]
        //[Route("image")]
        //public IActionResult DeleteAsset([FromForm]List<string> ids)
        //{
        //    try
        //    {
        //        _documentManager.DeleteAssetFromCloudinary(ids);
        //        return Ok(ids);
        //    }
        //    catch(Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        [Route("")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult NewActivity(ActivityWithPicModel activity)
        {
            try
            {
                if (activity != null)
                {
                    Activity act = new Activity();
                    act.ActivityName = activity.ActivityName;
                    act.IsActive = activity.ActivityIsActive;
                    act.Description = activity.Description;

                    int actId = _activityManager.PostActivity(act);

                    PicDocument pic = new PicDocument();
                    pic.PicType = "Activity";
                    pic.PicTypeId = actId;

                    if (activity?.ActivityPics?.Length > 0)
                    {
                        foreach (var item in activity.ActivityPics)
                        {
                            pic.PicValue = item.PicValue;
                            pic.PicName = item.PicName;
                            pic.CloudinaryPublicId = item.CloudinaryPublicId;

                            _documentManager.UploadImage(pic);
                        }
                    }

                    string requestUrl = HttpContext.Request.Path.ToString();
                    string responseBody = JsonConvert.SerializeObject(activity);

                    _log.setLogTrace(new HttpRequestMessage(), new HttpResponseMessage(), requestUrl, responseBody);

                }

                return Ok(activity);

            }
            catch (Exception ex)
            {
                _logger.LogError("An exception occurred while inserting activity data : " + ex.Message);
                return BadRequest(ex.Message);
            }

        }

    }
}
