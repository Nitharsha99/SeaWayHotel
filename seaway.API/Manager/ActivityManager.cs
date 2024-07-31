using Microsoft.Extensions.Configuration;
using seaway.API.Configurations;
using seaway.API.Models;
using seaway.API.Models.ViewModels;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using System.Drawing;
using System.IO;
using seaway.API.Models.Enum;

namespace seaway.API.Manager
{
    public class ActivityManager
    {
        private readonly ILogger<LogHandler> _logger;
        private readonly IConfiguration _configuration;
        string _conString;
        public ActivityManager(ILogger<LogHandler> logger, IConfiguration configuration) 
        {
            _logger = logger;
            _configuration = configuration;
            _conString = _configuration.GetConnectionString("DefaultConnection");
        }

        public List<Activity> GetActivities()
        {
            try
            {
                List<Activity> list = new List<Activity>();

                using(SqlConnection _con = new SqlConnection(this._conString))
                {
                    SqlCommand command = _con.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "GetAllActivities";                                  

                    _con.Open();

                    using(SqlDataReader reader = command.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            Activity activity = new Activity
                            {
                                ActivityId = (int)reader["ActivityId"],
                                ActivityName = reader["Name"].ToString(),
                                Description = reader["Description"].ToString(),
                                IsActive = Convert.ToBoolean(reader["IsActive"])
                            };

                            list.Add(activity);
                        }
                    }

                    _con.Close();

                    _logger.LogTrace("SuccessFully All Activity Data retrieved");

                    return list;
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(" Warning -- " + ex.Message);
                throw;
            }
        }

        public Activity GetActivityById(int activityId)
        {
            try
            {
                List<Activity> activityList = new List<Activity>();
                Activity mainActivity = new Activity();

                using (SqlConnection _con = new SqlConnection(this._conString))
                {
                    _con.Open();
                    using (var cmd = new SqlCommand("GetActivityById", _con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@activityId", activityId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var Id = (int)reader["ActivityId"];
                                Activity activity = activityList.FirstOrDefault(r => r.ActivityId == Id) ?? new Activity();

                                if (activity?.ActivityId == 0)
                                {
                                    activity = new Activity
                                    {
                                        ActivityId = (int)reader["ActivityId"],
                                        ActivityName = reader["ActivityName"].ToString(),
                                        Description = reader["Description"].ToString(),
                                        IsActive = (bool)reader["IsActive"]
                                    };

                                    activityList.Add(activity);
                                }

                                if (reader["PicName"] != DBNull.Value)
                                {
                                    byte[] picValueInByte = (byte[])reader["PicValue"];
                                    string val = Convert.ToBase64String(picValueInByte);

                                    PicDocument document = new PicDocument
                                    {
                                        PicName = reader["PicName"].ToString(),
                                        PicType = (PicType)reader["PicType"],
                                        PicTypeId = (int)reader["PicTypeId"],
                                        CloudinaryPublicId = reader["CloudinaryPublicId"].ToString(),
                                        PicValue = val
                                    };

                                    if (activity.ActivityPics == null)
                                    {
                                        activity.ActivityPics = new List<PicDocument>();
                                    }

                                    activity.ActivityPics.Add(document);
                                }
                            }
                        }
                    }

                    _con.Close();
                    _logger.LogTrace("SuccessFully Activity Data retrieved By Id : " + activityId);
                    mainActivity = activityList.FirstOrDefault() ?? new Activity();

                }
                return mainActivity;
            }
            catch (Exception e)
            {
                _logger.LogWarning(" Warning -- " + e.Message);
                throw;
            }
        }

        public int PostActivity(Activity activity)
        {
            try
            {
                int activityId = 0;
                using (SqlConnection _con = new SqlConnection(this._conString))
                {
                    using (SqlCommand command = new SqlCommand("InsertActivityWithPic", _con))
                    {
                        _con.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();

                        int active = (bool)activity.IsActive ? 1 : 0;

                        command.Parameters.AddWithValue("@ActivityName", activity.ActivityName);
                        command.Parameters.AddWithValue("@ActivityDescription", activity.Description);
                        command.Parameters.AddWithValue("@IsActive", active);
                        command.Parameters.AddWithValue("@createdBy", activity.CreatedBy);

                        activityId = (int)command.ExecuteScalar();
                        

                        //activityId = Convert.ToInt32(outputIdParam.Value);

                    }
                }

                _logger.LogTrace("Get Activity Id --> " + activityId);

                return activityId;
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Warning at Post Activity : " + ex.Message);
                throw;
            }

        }

        public bool DeleteActivity(int activityId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(this._conString))
                {
                    using (SqlCommand cmd = new SqlCommand("DeleteActivity", con))
                    {
                        con.Open();

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@activityId", activityId);

                        cmd.ExecuteNonQuery();

                        _logger.LogTrace("Sucessfully Deleted Activity of Id --> " + activityId + "From Database");

                        return true;

                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogWarning(" Warning -- " + e.Message);
                return false;
            }
        }

        public void UpdateActivity(Activity activity, int activityId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(this._conString))
                {
                    using (SqlCommand cmd = new SqlCommand("UpdateActivity", con))
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@ActivityId", activityId);
                        cmd.Parameters.AddWithValue("@Name", activity.ActivityName);
                        cmd.Parameters.AddWithValue("@Description", activity.Description);
                        cmd.Parameters.AddWithValue("@IsActive", activity.IsActive);
                        cmd.Parameters.AddWithValue("@UpdatedBy", activity.UpdatedBy);
                     

                        cmd.ExecuteNonQuery();
                    }
                }

                _logger.LogTrace("SuccessFully updated the Activity");
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Warning at Update the " + activity.ActivityName + " : " + ex.Message);
                throw;
            }
        }

        public bool ChangeActiveStatus(bool status, int id)
        {
            try
            {
                var query = "UPDATE Activities SET IsActive = @status WHERE ActivityId = @Id";

                using (SqlConnection con = new SqlConnection(this._conString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        con.Open();

                        cmd.Parameters.AddWithValue("@Id", id);
                        cmd.Parameters.AddWithValue("@status", status);

                        cmd.ExecuteNonQuery();

                        _logger.LogTrace("Sucessfully Changed Activity Status of Id --> " + id + "From Database");

                        return true;

                    }
                }
            }
            catch (SqlException ex)
            {
                _logger.LogWarning(" Warning -- " + ex.Message);
                return false;
            }
        }

        public bool IsUsernameExist(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }
            else
            {
                var result = GetActivities()
                    .Where(a => a.ActivityName.Trim().ToLower() == name.Trim().ToLower() && a.IsActive)
                    .ToList();

                if (result.Count() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsNameChange(string inputName, string oldName)
        {
            if (string.IsNullOrEmpty(inputName) || string.IsNullOrEmpty(oldName)) { return false; }
            else
            {
                bool isNameChange = inputName.Trim().ToLower() != oldName.Trim().ToLower();
                return isNameChange;
            }
        }

    }
}
