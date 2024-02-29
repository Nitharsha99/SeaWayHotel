using Microsoft.Extensions.Configuration;
using seaway.API.Configurations;
using seaway.API.Models;
using seaway.API.Models.ViewModels;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

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

        public List<ActivityWithPicModel> GetActivities()
        {
            try
            {
                List<ActivityWithPicModel> list = new List<ActivityWithPicModel>();

                using(SqlConnection _con = new SqlConnection(this._conString))
                {
                    SqlCommand command = _con.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "AllAdmin";                                   //TODO: change SP name
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();

                    _con.Open();
                    adapter.Fill(dt);
                    _con.Close();

                    foreach (DataRow row in dt.Rows)
                    {
                        byte[] Pics = (byte[])row["Picture"];
                        list.Add(new ActivityWithPicModel
                        {
                            ActivityName = row["Name"].ToString(),
                            Description = row["Description"].ToString(),
                        //    Pics = Convert.ToBase64String(Pics),
                            //ActivityIsActive = Convert.ToBoolean(row["IsActive"])
                        });
                    }

                    _logger.LogTrace("SuccessFully All Activity Data retrieved");

                    return list;
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Warning -- " + ex.Message);
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
