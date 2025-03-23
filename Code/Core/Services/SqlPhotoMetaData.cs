using Microsoft.Data.SqlClient;
using Core.Interfaces;
using Core.Models;
using System.Data; 

namespace Core.Services;

public class SqlPhotoMetaData : IPhotoMetaData
{
    private readonly IConfiguration _configuration;

    public SqlPhotoMetaData(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public async Task<List<PhotoModel>> GetUserPhotos(string userName)
    {
        ArgumentNullException.ThrowIfNull(userName, nameof(userName));

        string connectionString = _configuration.GetConnectionString("Default")
            ?? throw new InvalidOperationException("Connection string 'Default' not found.");

        var result = new List<PhotoModel>();
        await using var conn = new SqlConnection(connectionString);
        const string spName = "GetUserPhotos";
        await using var command = new SqlCommand(spName, conn)
        {
            CommandType = CommandType.StoredProcedure
        };
        command.Parameters.AddWithValue("@UserName", userName);

        await conn.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            result.Add(new PhotoModel
            {
                Description = reader.GetString(reader.GetOrdinal("Description")),
                FileName = reader.GetString(reader.GetOrdinal("FileName"))
            });
        }

        return result;
    }

    public async Task SavePhotoMetaData(string userName, string description, string fileName)
    {
        ArgumentNullException.ThrowIfNull(userName, nameof(userName));
        ArgumentNullException.ThrowIfNull(description, nameof(description));
        ArgumentNullException.ThrowIfNull(fileName, nameof(fileName));

        string connectionString = _configuration.GetConnectionString("Default")
            ?? throw new InvalidOperationException("Connection string 'Default' not found.");

        await using var conn = new SqlConnection(connectionString);
        const string spName = "SaveMetaData";
        await using var command = new SqlCommand(spName, conn)
        {
            CommandType = CommandType.StoredProcedure
        };
        command.Parameters.AddWithValue("@UserName", userName);
        command.Parameters.AddWithValue("@Description", description);
        command.Parameters.AddWithValue("@FileName", fileName);

        await conn.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }
}