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

        public async Task<bool> DeleteAssetFromCloudinary(string[] Ids)
        {
            try
            {
                var deleteParams = new DelResParams()
                {
                    PublicIds = Ids.ToList(),
                    Type = "upload",
                    ResourceType = ResourceType.Image
                };

                var result = await this._cloudinary.DeleteResourcesAsync(deleteParams);

                _logger.LogTrace("Sucessfully Deleted PicPublic -- " + result );
                return true;
            }
            catch(Exception ex)
            {
                _logger.LogWarning("Warning -- " + ex.Message);
                return false;
            }
           
        }

        public async void DeleteImageFromDB(string[] Ids)
        {
            var query = "DELETE FROM PicDocuments WHERE CloudinaryPublicId= @Id AND PicType = 'Room'";

            try
            {
                using (SqlConnection con = new SqlConnection(this._conString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        foreach (var Id in Ids)
                        {
                            cmd.Parameters.AddWithValue("@Id", Id);

                            con.Open();
                            cmd.ExecuteNonQuery();

                            _logger.LogTrace("Sucessfully Deleted PicPublic Id --> " + Id + "From Database");

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Warning -- " + ex.Message);
                throw;
            }

        }

    }
}
