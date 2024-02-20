using Microsoft.Extensions.Configuration;
using seaway.API.Configurations;
using seaway.API.Models;
using seaway.API.Models.ViewModels;
using System.Data;
using System.Data.SqlClient;

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
                            Pics = Convert.ToBase64String(Pics),
                            ActivityIsActive = Convert.ToBoolean(row["IsActive"])
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

        //public ActivityWithPicModel PostActivity()
        //{

        //}
    }
}
