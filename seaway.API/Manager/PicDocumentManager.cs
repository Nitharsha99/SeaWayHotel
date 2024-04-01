using seaway.API.Configurations;
using seaway.API.Models;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

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

        public async void UploadImage(PicDocument pic)
        {
            try
            {
                if (pic != null)
                {
                    byte[] fileBytes = null;
                    //if (System.IO.File.Exists(pic.PicValue))
                    //{
                    //    fileBytes = await System.IO.File.ReadAllBytesAsync(pic.PicValue);
                    //}

                    using (SqlConnection _con = new SqlConnection(this._conString))
                    {
                        await _con.OpenAsync();

                        using (SqlTransaction transaction = _con.BeginTransaction())
                        {
                            using (MemoryStream stream = new MemoryStream(fileBytes))
                            {

                                //string sql = "Insert into PicDocuments(PicTypeId, PicType, Name, Value, InsertedTime) Values(@TypeId, @Type, @Name, @Value, GETDATE())";
                                using (SqlCommand command = new SqlCommand("UploadImage", _con, transaction))
                                {
                                    command.CommandType = CommandType.StoredProcedure;

                                    command.Parameters.AddWithValue("@PicTypeId", pic.PicTypeId);
                                    command.Parameters.AddWithValue("@PicType", pic.PicType);
                                    command.Parameters.AddWithValue("@PicName", pic.PicName);
                                    //command.Parameters.AddWithValue("@PicValue", stream.ToArray());
                                    await command.ExecuteNonQueryAsync();
                                }

                            }

                            transaction.Commit();
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


        //[Obsolete]
        //public ImageUploadResponse UploadImageToCloudinary(IFormFile file)
        //{
        //    try
        //    {
        //        if (file == null || file.Length == 0)
        //        {
        //            throw new ArgumentException("File is not provided or is empty.");
        //        }

        //        using (var stream = file.OpenReadStream())
        //        {
        //            var uploadParams = new ImageUploadParams
        //            {
        //                File = new FileDescription(file.FileName, stream)
        //            };

        //            var uploadResult = _cloudinary.Upload(uploadParams);

        //            return new ImageUploadResponse
        //            {
        //                PublicId = uploadResult.PublicId,
        //                Url = uploadResult.SecureUri.AbsoluteUri
        //            };
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}
    }
}
