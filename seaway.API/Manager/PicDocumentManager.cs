using seaway.API.Configurations;
using seaway.API.Models;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System.Transactions;
using System.Text;

namespace seaway.API.Manager
{
    public class PicDocumentManager
    {
        private readonly ILogger<LogHandler> _logger;
        private readonly IConfiguration _configuration;
        string _conString;
        private readonly Cloudinary _cloudinary;

        public PicDocumentManager(ILogger<LogHandler> logger, IConfiguration configuration)
        {
            _logger = logger;
           _configuration = configuration;
            _conString = _configuration.GetConnectionString("DefaultConnection");
            _cloudinary = new Cloudinary("cloudinary://282416579661484:CMQR4xZaviBjuveFI9Ayk8orhBg@dly7yjg1w");
            _cloudinary.Api.Secure = true;
           
        }

        public void UploadImage(PicDocument pic)
        {
            try
            {
                if (pic != null)
                {
                    byte[] picValueBytes = Encoding.UTF8.GetBytes(pic.PicValue);

                    using (SqlConnection _con = new SqlConnection(this._conString))
                    {
                        using(SqlCommand command = new SqlCommand("UploadImage", _con))
                        {
                            _con.Open();
                            command.CommandType = CommandType.StoredProcedure;

                            command.Parameters.AddWithValue("@PicTypeId", pic.PicTypeId);
                            command.Parameters.AddWithValue("@PicType", pic.PicType);
                            command.Parameters.AddWithValue("@PicName", pic.PicName);
                            command.Parameters.AddWithValue("PicValue", picValueBytes);
                            command.Parameters.AddWithValue("PublicId", pic.CloudinaryPublicId);

                            command.ExecuteNonQuery();
                        }
                    }
                }

                _logger.LogTrace("Sucessfully Upload" + pic?.PicName + " Image ");
            }
            catch(Exception ex) 
            {
                _logger.LogWarning("Warning -- " + ex.Message);
                throw;
            }
        }

        public async void DeleteAssetFromCloudinary(List<string> Ids)
        {
            List<string> AssetIds = new List<string>();
            foreach (var asset in Ids)
            {
                string id = "Seaway/" + asset;

                AssetIds.Add(id);
            }
            var deleteParams = new DelResParams()
            {
                PublicIds = AssetIds,
                Type = "upload",
                ResourceType = ResourceType.Image
            };

            var result = this._cloudinary.DeleteResources(deleteParams);
            Console.WriteLine(result.JsonObj);
        }

    }
}
