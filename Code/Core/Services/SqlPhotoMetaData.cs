using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Models;
using Microsoft.Extensions.Configuration;

namespace Core.Services
{
    public class SqlPhotoMetaData : IPhotoMetaData
    {
        private readonly IConfiguration _configuration;

        public SqlPhotoMetaData(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<PhotoModel>> GetUserPhotos(string userName)
        {
            var connectionString = _configuration.GetConnectionString("Default");

            var result = new List<PhotoModel>();
            using (var conn = new SqlConnection(connectionString))
            {
                const string spName = "GetUserPhotos";
                var command = conn.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = spName;
                command.Parameters.AddWithValue("@UserName", userName);
                await conn.OpenAsync();
                var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                    result.Add(new PhotoModel
                    {
                        Description = reader.GetString(reader.GetOrdinal("Description")),
                        FileName = reader.GetString(reader.GetOrdinal("FileName"))
                    });
            }

            return result;
        }

        public async Task SavePhotoMetaData(string userName, string desciption, string fileName)
        {
            var connectionString = _configuration.GetConnectionString("Default");
            using (var conn = new SqlConnection(connectionString))
            {
                const string spName = "SaveMetaData";
                var command = conn.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = spName;
                command.Parameters.AddWithValue("@userName", userName);
                command.Parameters.AddWithValue("@Description", desciption);
                command.Parameters.AddWithValue("@fileName", fileName);
                await conn.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}