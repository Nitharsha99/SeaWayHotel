using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using seaway.API.Configurations;
using seaway.API.Manager;
using seaway.API.Models;
using seaway.API.Models.ViewModels;
using System;
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


        [HttpDelete]
        [Route("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteActivity([FromRoute] int Id)
        {
            try
            {
                Activity activity = new Activity();
                bool isActivityRemove = false;

                activity = _activityManager.GetActivityById(Id);

                if (activity.ActivityId != null)
                {

                    isActivityRemove = _activityManager.DeleteActivity(Id);

                    string requestUrl = HttpContext.Request.Path.ToString();
                    string responseBody = JsonConvert.SerializeObject(activity);

                    _log.setLogTrace(new HttpRequestMessage(), new HttpResponseMessage(), requestUrl, responseBody);

                    if (isActivityRemove)
                    {
                        return Ok(activity);
                    }
                    else
                    {
                        return BadRequest("Issue in deleting process");
                    }
                }
                else
                {
                    return BadRequest("No Activity exist for this Id");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("An exception occurred while deleting Activity : " + ex.Message);
                return BadRequest(ex.Message);
            }
        }


        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateActivity(ActivityWithPicModel activity, int activityId)
        {
            try
            {
                if ((activity.ActivityName != null || activity.Description != null|| activity?.ActivityIsActive != null) && (activityId != 0))
                {
                    Activity oldActivity = _activityManager.GetActivityById(activityId);
                    if (oldActivity.ActivityName != null)
                    {

                        Activity updateActivity = new Activity
                        {
                            ActivityName = activity.ActivityName,
                            Description = activity.Description,
                            IsActive = activity.ActivityIsActive,
                 
                        };
                        _activityManager.UpdateActivity(updateActivity, activityId);


                        PicDocument pic = new PicDocument();
                        pic.PicType = "Activity";
                        pic.PicTypeId = activityId;
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
                        string responseBody = JsonConvert.SerializeObject(updateActivity);

                        _log.setLogTrace(new HttpRequestMessage(), new HttpResponseMessage(), requestUrl, responseBody);

                    }
                    else
                    {
                        return BadRequest("Invalid ActivityId");
                    }
                }

                return Ok(activity);
            }
            catch (Exception ex)
            {
                _logger.LogError("An exception occurred while updating Activity data : " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ChangeActiveStatus(bool status, int id)
        {
            try
            {
                if (id != 0)
                {
                    bool isStatusChanged = false;
                    Activity activity = _activityManager.GetActivityById(id);

                    if (activity?.ActivityName != null)
                    {
                        isStatusChanged = _activityManager.ChangeActiveStatus(status, id);

                        if (isStatusChanged)
                        {
                            return Ok(true);
                        }
                        else
                        {
                            return BadRequest("Error on changing activity status");
                        }
                    }
                    else
                    {
                        return BadRequest("There is no any activity with this Id --> " + id);
                    }
                }
                return BadRequest("Id is not Supplied");
            }
            catch (Exception ex)
            {
                _logger.LogError("An exception occurred while changing Active status of Activity : " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("image")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteAsset([FromQuery] string ids)
        {
            try
            {
                List<string> idArray = new List<string>();
                idArray = ids.Split(',').ToList();
                bool IsRemoveFromCLoudinary = false;

                IsRemoveFromCLoudinary = _documentManager.DeleteAssetFromCloudinary(idArray).Result;

                if (IsRemoveFromCLoudinary)
                {
                    string picType = "Activity";
                    _documentManager.DeleteImageFromDB(idArray, picType);
                }

                string requestUrl = HttpContext.Request.Path.ToString();
                string responseBody = JsonConvert.SerializeObject(ids);

                _log.setLogTrace(new HttpRequestMessage(), new HttpResponseMessage(), requestUrl, responseBody);
                return Ok("Deleted " + responseBody);
            }
            catch (Exception ex)
            {
                _logger.LogError("An exception occurred while deleting room pictures : " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

    }
}
