﻿using Microsoft.AspNetCore.Components.Forms;
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
        public IActionResult PostActivity([FromForm]ActivityWithPicModel activity)
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

                    if (activity.PicValue != null)
                    {
                        foreach (var value in activity.PicValue)
                        {
                            //pic.PicName = Path.GetFileName(value);
                            //pic.PicValue = value;
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
        public IActionResult UpdateActivity([FromForm] Activity activity, int activityId)
        {
            try
            {
                if ((activity.ActivityName != null || activity.Description != null|| activity.IsActive != null) && (activityId != 0))
                {
                    Activity oldActivity = _activityManager.GetActivityById(activityId);
                    if (oldActivity.ActivityName != null)
                    {
                        Activity updateActivity = new Activity
                        {
                            ActivityName = activity.ActivityName,
                            Description = activity.Description,
                            IsActive = activity.IsActive,
                 
                        };
                        _activityManager.UpdateActivity(updateActivity, activityId);

                        
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

    }
}
