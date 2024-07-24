using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using seaway.API.Configurations;
using seaway.API.Manager;
using seaway.API.Models;
using seaway.API.Models.ViewModels;
using System;

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
                _logger.LogError(LogMessages.GetActivityDataError + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult FindActivityById([FromRoute] int Id)
        {
            try
            {
                if(Id > 0)
                {
                    Activity activity = _activityManager.GetActivityById(Id);

                    string responseBody = JsonConvert.SerializeObject(activity);

                    string requestUrl = HttpContext.Request.Path.ToString();

                    _log.setLogTrace(new HttpRequestMessage(), new HttpResponseMessage(), responseBody, requestUrl);

                    if(activity.ActivityName != null)
                    {
                        return Ok(activity);
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
                _logger.LogError(LogMessages.FindActivityByIdError + Id + " : " + ex.Message);
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
                if (activity.ActivityName != null && activity.Description != null)
                {
                    Activity act = new Activity();
                    act.ActivityName = activity.ActivityName;
                    act.IsActive = activity.IsActive;
                    act.Description = activity.Description;
                    act.CreatedBy = activity.CreatedBy;

                    int actId = _activityManager.PostActivity(act);

                    PicDocument pic = new PicDocument();
                    pic.PicType = "Activity";
                    pic.PicTypeId = actId;
                    pic.CreatedBy = activity.CreatedBy;

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
                    return Ok(activity);

                }
                else
                {
                    return BadRequest(DisplayMessages.NullInput);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(LogMessages.InsertDataError + ex.Message);
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
                if(Id > 0)
                {
                    Activity activity = new Activity();
                    bool isActivityRemove = false;

                    activity = _activityManager.GetActivityById(Id);

                    if (activity?.ActivityId != null)
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
                            return BadRequest(DisplayMessages.DeletingError);
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
            catch (Exception ex)
            {
                _logger.LogError(LogMessages.DeleteActivityError + ex.Message);
                return BadRequest(ex.Message);
            }
        }


        [HttpPut]
        [Route("{activityId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateActivity(ActivityWithPicModel activity, int activityId)
        {
            try
            {
                if ((activity.ActivityName != null || activity.Description != null|| activity?.IsActive != null) && (activityId != 0))
                {
                    Activity oldActivity = _activityManager.GetActivityById(activityId);
                    if (oldActivity.ActivityName != null)
                    {

                        Activity updateActivity = new Activity
                        {
                            ActivityName = activity.ActivityName,
                            Description = activity.Description,
                            IsActive = activity.IsActive,
                            UpdatedBy = activity.UpdatedBy,
                        };
                        _activityManager.UpdateActivity(updateActivity, activityId);


                        PicDocument pic = new PicDocument();
                        pic.PicType = "Activity";
                        pic.PicTypeId = activityId;
                        pic.CreatedBy = activity.UpdatedBy;
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

                        return Ok(activity);

                    }
                    else
                    {
                        return NotFound(DisplayMessages.EmptyExistData + activityId);
                    }
                }
                else
                {
                    return BadRequest(DisplayMessages.NullInput);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(LogMessages.UpdateDataError + ex.Message);
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
                if (id > 0)
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
                            return BadRequest(DisplayMessages.StatusChangeError);
                        }
                    }
                    else
                    {
                        return NotFound(DisplayMessages.EmptyExistData + id);
                    }
                }
                else
                {
                    return BadRequest(DisplayMessages.InvalidId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(LogMessages.StatusChangeError + ex.Message);
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
                if (!string.IsNullOrEmpty(ids))
                {
                    List<string> idArray = new List<string>();
                    idArray = ids.Split(',').ToList();
                    bool IsRemoveFromCLoudinary = false;

                    IsRemoveFromCLoudinary = _documentManager.DeleteAssetFromCloudinary(idArray).Result;

                    if (IsRemoveFromCLoudinary)
                    {
                        string picType = "Activity";
                        _documentManager.DeleteImageFromDB(idArray, picType);


                        string requestUrl = HttpContext.Request.Path.ToString();
                        string responseBody = JsonConvert.SerializeObject(ids);

                        _log.setLogTrace(new HttpRequestMessage(), new HttpResponseMessage(), requestUrl, responseBody);
                        return Ok("Deleted " + responseBody);
                    }
                    else
                    {
                        return BadRequest(DisplayMessages.CloudinaryError);
                    }
                }
                else
                {
                    return BadRequest(DisplayMessages.NullInput);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(LogMessages.DeleteImageError + ex.Message);
                return BadRequest(ex.Message);
            }
        }

    }
}
