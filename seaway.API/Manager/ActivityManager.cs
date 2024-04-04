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

                            if (reader["PicName"] != DBNull.Value)
                            {
                                byte[] picValueInByte = (byte[])reader["PicValue"];
                                string val = Convert.ToBase64String(picValueInByte);

                                //using (MemoryStream ms = new MemoryStream(picValueInByte))
                                //{
                                //    System.Drawing.Image image = Image.FromStream(ms);
                                //}

                                PicDocument document = new PicDocument
                                {
                                    PicName = reader["PicName"].ToString(),
                                    PicType = reader["PicType"].ToString(),
                                    //PicValue = val
                                    //PicValueInByte = picValueInByte,
                                   
                                };

                                if(activity.ActivityPics == null)
                                {
                                    activity.ActivityPics = new List<PicDocument>();
                                }

                                activity.ActivityPics.Add(document);

                            }

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

    }
}
