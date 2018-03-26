using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Core.Services
{
    public class SqlServerLogins : ILogins
    {
        private readonly string _connectionString;
        private readonly ILogger _logger;
        public SqlServerLogins(IConfiguration configuration, ILogger<SqlServerLogins> logger
        )
        {
            _connectionString = configuration.GetConnectionString("Default");
            _logger = logger;
        }

        public async Task CreateUser(string email, string password)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "CreateUser";
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Password", password);
                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();

                    _logger.LogInformation($"A user with email address of {email} was registered.");
                }
            }
            catch (Exception e)
            {
                _logger.LogWarning(e,"Error in CreateUser method");
                throw;
            }
            
        }

        public async Task<User> GetUser(string email)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "GetUser";
                command.Parameters.AddWithValue("@Email", email);
                await connection.OpenAsync();
                var reader = await command.ExecuteReaderAsync();

                User result = null;
                if (await reader.ReadAsync())
                    result = new User
                    {
                        Email = reader.GetString(reader.GetOrdinal("Email")),
                        Password = reader.GetString(reader.GetOrdinal("Password"))
                    };

                return result;
            }
        }
    }
}