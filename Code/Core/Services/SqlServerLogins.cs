using Microsoft.Data.SqlClient;
using System.Data; 
using Core.Interfaces;
using Core.Models;
using System.ComponentModel.DataAnnotations;


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

    public async Task CreateUser(string email, string password, CancellationToken token)
    {
        if (string.IsNullOrEmpty(email))
        {
            throw new ArgumentException("Value cannot be null or empty.", nameof(email));
        }

        if (string.IsNullOrEmpty(password))
        {
            throw new ArgumentException("Value cannot be null or empty.", nameof(password));
        }

        var emailValidator = new EmailAddressAttribute();
        if (!emailValidator.IsValid(email))
        {
            throw new ArgumentException("The email address is not in a valid format.", nameof(email));
        }

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

            await connection.OpenAsync(token);
            await command.ExecuteNonQueryAsync(token);

            _logger.LogInformation("A user with email address {Email} was registered.", email);
        }
        catch (Exception e)
        {
            _logger.LogWarning(e, "Error creating user with email {Email}", email);
            throw;
        }
    }

    public async Task<UserLoginResult> Login(string email, string password, CancellationToken token)
    {
        if (string.IsNullOrEmpty(email))
        {
            throw new ArgumentException("Value cannot be null or empty.", nameof(email));
        }

        if (string.IsNullOrEmpty(password))
        {
            throw new ArgumentException("Value cannot be null or empty.", nameof(password));
        }

        var emailValidator = new EmailAddressAttribute();
        if (!emailValidator.IsValid(email))
        {
            throw new ArgumentException("The email address is not in a valid format.", nameof(email));
        }
        
        await using var connection = new SqlConnection(_connectionString);
        const string spName = "Login";
        await using var command = new SqlCommand(spName, connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        command.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 256) { Value = email });
        command.Parameters.Add(new SqlParameter("@Password", SqlDbType.NVarChar, 256) { Value = password });

        await connection.OpenAsync(token);
        await using var reader = await command.ExecuteReaderAsync(token);

        if (await reader.ReadAsync(token))
        {
            return UserLoginResult.Success;
        }

        return UserLoginResult.InvalidPassword;
    }
    public async Task<User?> GetUser(string email, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(email, nameof(email));

        await using var connection = new SqlConnection(_connectionString);
        const string spName = "GetUser";
        await using var command = new SqlCommand(spName, connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        command.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 256) { Value = email });

        await connection.OpenAsync(token);
        await using var reader = await command.ExecuteReaderAsync();

        User? result = null;
        if (await reader.ReadAsync(token))
        {
            result = new User
            {
                Email = reader.GetString(reader.GetOrdinal("Email")) 
            };
        }

        return result;
    }

}

