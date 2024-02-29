﻿using seaway.API.Configurations;
using seaway.API.Models;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System;

namespace seaway.API.Manager
{
    public class PicDocumentManager
    {
        private readonly ILogger<LogHandler> _logger;
        private readonly IConfiguration _configuration;
        string _conString;

        public PicDocumentManager(ILogger<LogHandler> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _conString = _configuration.GetConnectionString("DefaultConnection");
        }

        public async void UploadImage(PicDocument pic)
        {
            try
            {
                if (pic != null)
                {
                    byte[] fileBytes = null;
                    if (System.IO.File.Exists(pic.PicValue))
                    {
                        fileBytes = await System.IO.File.ReadAllBytesAsync(pic.PicValue);
                    }

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
                                    command.Parameters.AddWithValue("@PicValue", stream.ToArray());
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
    }
}
