using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using seaway.API.Configurations;
using seaway.API.Manager;
using seaway.API.Models;
using seaway.API.Models.Enum;
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
        public async Task<IActionResult> GetAllActivities()
        {
            try
            {
                List<Activity> activities = await _activityManager.GetActivities();

                string responseBody = JsonConvert.SerializeObject(activities);

                string requestUrl = HttpContext.Request.Path.ToString();

                _log.setLogTrace(new HttpRequestMessage(), new HttpResponseMessage(), responseBody, requestUrl);

                return Ok(activities);
            }
            catch (Exception ex)
            {
                _logger.LogError(LogMessages.GetActivityDataError + ex.Message);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet]
        [Route("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> FindActivityById([FromRoute] int Id)
        {
            try
            {
                if(Id > 0)
                {
                    Activity activity = await _activityManager.GetActivityById(Id);

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
                _logger.LogError(LogMessages.FindDataByIdError + Id + " : " + ex.Message);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [Route("")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> NewActivity(ActivityWithPicModel activity)
        {
            try
            {
                bool isNameExist = false;
                if (activity.ActivityName != null)
                {
                    isNameExist = await _activityManager.IsUsernameExist(activity.ActivityName);

                    if (isNameExist)
                    {
                        return BadRequest(DisplayMessages.ExistNameError);
                    }
                    else
                    {
                        Activity act = new Activity
                        {
                            ActivityName = activity.ActivityName,
                            IsActive = activity.IsActive,
                            Description = activity.Description,
                            CreatedBy = activity.CreatedBy
                        };
                       
                        int actId = await _activityManager.PostActivity(act);

                        if(actId > 0)
                        {
                            if (activity?.ActivityPics?.Length > 0)
                            {
                                PicDocument pic = new PicDocument
                                {
                                    PicType = PicType.Activity,
                                    PicTypeId = actId,
                                    CreatedBy = activity.CreatedBy
                                };

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
                            return StatusCode(500, "Internal Server Error");
                        }

                    }
                }
                else
                {
                    return BadRequest(DisplayMessages.NullInput);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(LogMessages.InsertDataError + ex.Message);
                return StatusCode(500, "Internal Server Error");
            }

        }


        [HttpDelete]
        [Route("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteActivity([FromRoute] int Id)
        {
            try
            {
                if(Id > 0)
                {
                    Activity activity = new Activity();
                    bool isActivityRemove = false;
                    List<string> publicIds = new List<string>();
                    bool IsRemoveFromCLoudinary = false;

                    activity = await _activityManager.GetActivityById(Id);

                    if (activity?.ActivityId > 0)
                    {
                        if(activity?.ActivityPics?.Count > 0)
                        {
                            foreach (var pic in activity.ActivityPics)
                            {
                                publicIds.Add(pic.CloudinaryPublicId ?? "");
                            }
                            IsRemoveFromCLoudinary = _documentManager.DeleteAssetFromCloudinary(publicIds).Result;

                            if (IsRemoveFromCLoudinary)
                            {
                                isActivityRemove = await _activityManager.DeleteActivity(Id);
                            }
                        }
                        else
                        {
                            isActivityRemove = await _activityManager.DeleteActivity(Id);
                        }

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
                return StatusCode(500, "Internal Server Error");
            }
        }


        [HttpPut]
        [Route("{activityId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateActivity(ActivityWithPicModel activity, int activityId)
        {
            try
            {
                if ((activity.ActivityName != null || activity.Description != null|| activity?.IsActive != null) && (activityId > 0))
                {
                    bool isNameExist = false;
                    bool isNameChange = false;
                    Activity oldActivity = await _activityManager.GetActivityById(activityId);
                    if (oldActivity.ActivityName != null)
                    {
                        if (activity.ActivityName != null)
                        {
                            isNameChange = _activityManager.IsNameChange(activity.ActivityName, oldActivity.ActivityName);

                            if (isNameChange)
                            {
                                isNameExist = await _activityManager.IsUsernameExist(activity.ActivityName);
                            }
                        }
                        if (isNameExist)
                        {
                            return BadRequest(DisplayMessages.ExistNameError);
                        }
                        else
                        {

                            Activity updateActivity = new Activity
                            {
                                ActivityName = activity.ActivityName,
                                Description = activity.Description,
                                IsActive = activity.IsActive,
                                UpdatedBy = activity.UpdatedBy,
                            };
                            _activityManager.UpdateActivity(updateActivity, activityId);


                            if (activity?.ActivityPics?.Length > 0)
                            {
                                PicDocument pic = new PicDocument()
                                {
                                    PicType = PicType.Activity,
                                    PicTypeId = activityId,
                                    CreatedBy = activity.UpdatedBy
                                };

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
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPatch]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangeActiveStatus(Activity act)
        {
            try
            {
                if (act.ActivityId > 0)
                {
                    bool isStatusChanged = false;
                    Activity activity = await _activityManager.GetActivityById(act.ActivityId);

                    if (activity?.ActivityName != null)
                    {
                        isStatusChanged = await _activityManager.ChangeActiveStatus(act.IsActive, act.ActivityId);

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
                        return NotFound(DisplayMessages.EmptyExistData + act.ActivityId);
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
                return StatusCode(500, "Internal Server Error");
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
                    bool isDeleted = false;

                    IsRemoveFromCLoudinary = _documentManager.DeleteAssetFromCloudinary(idArray).Result;

                    if (IsRemoveFromCLoudinary)
                    {
                        int picType = 2;
                        isDeleted = _documentManager.DeleteImageFromDB(idArray, picType).Result;

                        if(!isDeleted)
                        {
                            return BadRequest(DisplayMessages.ImageDeleteError);
                        }
                        else
                        {
                            string requestUrl = HttpContext.Request.Path.ToString();
                            string responseBody = JsonConvert.SerializeObject(ids);

                            _log.setLogTrace(new HttpRequestMessage(), new HttpResponseMessage(), requestUrl, responseBody);
                            return Ok("Deleted " + responseBody);
                        }
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
                return StatusCode(500, "Internal Server Error");
            }
        }

    }
}
