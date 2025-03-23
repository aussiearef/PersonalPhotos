using Microsoft.Data.SqlClient;
using System.Data; 
using Core.Interfaces;
using Core.Models;


namespace Core.Services;

public class SqlServerLogins : ILogins
{
    private readonly string _connectionString;
    private readonly ILogger<SqlServerLogins> _logger;

    public SqlServerLogins(IConfiguration configuration, ILogger<SqlServerLogins> logger)
    {
        ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        _connectionString = configuration.GetConnectionString("Default")
            ?? throw new InvalidOperationException("Connection string 'Default' not found.");
        _logger = logger;
    }

    public async Task CreateUser(string email, string password)
    {
        ArgumentNullException.ThrowIfNull(email, nameof(email));
        ArgumentNullException.ThrowIfNull(password, nameof(password));

        try
        {
            await using var connection = new SqlConnection(_connectionString);
            const string spName = "CreateUser";
            await using var command = new SqlCommand(spName, connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@Email", email);
            command.Parameters.AddWithValue("@Password", password);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();

            _logger.LogInformation("A user with email address {Email} was registered.", email);
        }
        catch (Exception e)
        {
            _logger.LogWarning(e, "Error creating user with email {Email}", email);
            throw;
        }
    }

    public async Task<User?> GetUser(string email)
    {
        ArgumentNullException.ThrowIfNull(email, nameof(email));

        await using var connection = new SqlConnection(_connectionString);
        const string spName = "GetUser";
        await using var command = new SqlCommand(spName, connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        command.Parameters.AddWithValue("@Email", email);

        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();

        User? result = null;
        if (await reader.ReadAsync())
        {
            result = new User
            {
                Email = reader.GetString(reader.GetOrdinal("Email")),
                Password = reader.GetString(reader.GetOrdinal("Password"))
            };
        }

        return result;
    }
}