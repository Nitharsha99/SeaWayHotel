﻿using seaway.API.Configurations;
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

        public async void UploadImage(PicDocument pic)
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
                            await _con.OpenAsync();
                            command.CommandType = CommandType.StoredProcedure;

                            command.Parameters.AddWithValue("@PicTypeId", pic.PicTypeId);
                            command.Parameters.AddWithValue("@PicType", pic.PicType);
                            command.Parameters.AddWithValue("@PicName", pic.PicName);
                            command.Parameters.AddWithValue("@PicValue", picValueBytes);
                            command.Parameters.AddWithValue("@PublicId", pic.CloudinaryPublicId);
                            command.Parameters.AddWithValue("@CreatedBy", pic.CreatedBy);

                            await command.ExecuteNonQueryAsync();
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

        public async Task<bool> DeleteAssetFromCloudinary(List<string> Ids)
        {
            try
            {
                List<string> deleteAsset = new List<string>();
                foreach (var item in Ids)
                {
                    var assets = _cloudinary.GetResource(item);

                    if(assets != null)
                    {
                        deleteAsset.Add(item);
                    }
                    else
                    {
                        _logger.LogWarning("Warning --> " + item + " is not in the Cloudinary");
                    }
                }
                
                if(deleteAsset.Count > 0)
                {
                    var deleteParams = new DelResParams()
                    {
                        PublicIds = deleteAsset.ToList(),
                        Type = "upload",
                        ResourceType = ResourceType.Image
                    };

                    var result = await this._cloudinary.DeleteResourcesAsync(deleteParams);
                    _logger.LogTrace("Sucessfully Deleted PicPublic -- " + result);
                }
               
                return true;
            }
            catch(Exception ex)
            {
                _logger.LogWarning("Warning -- " + ex.Message);
                return false;
            }
           
        }

        public async Task<bool> DeleteImageFromDB(List<string> Ids, int picType)
        {
            var query = "DELETE FROM PicDocuments WHERE CloudinaryPublicId= @Id AND PicType = @PicType";

            try
            {
                using (SqlConnection con = new SqlConnection(this._conString))
                {

                    if (con.State != ConnectionState.Open)
                    {
                        await con.OpenAsync();
                    }

                    foreach (var Id in Ids)
                    {
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.Add("@Id", SqlDbType.NVarChar).Value = Id.Trim();
                            cmd.Parameters.Add("@PicType", SqlDbType.Int).Value = picType;

                            await cmd.ExecuteNonQueryAsync();

                            _logger.LogTrace("Successfully Deleted PicPublic Id --> " + Id + "From Database");
                        }
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Warning -- " + ex.Message);
                return false;
            }

        }

    }
}
