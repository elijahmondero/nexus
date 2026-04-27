using System.Data;
using Microsoft.Data.SqlClient;
using MySqlConnector;
using Npgsql;

namespace Nexus.Api.Data;

public class DbConnectionFactory
{
    private readonly IConfiguration _config;

    public DbConnectionFactory(IConfiguration config)
    {
        _config = config;
    }

    public string Provider => _config["DB_PROVIDER"]?.ToLower() ?? "postgres";
    public string ConnectionString => _config["DB_CONNECTION_STRING"] ?? "";

    public IDbConnection CreateConnection()
    {
        return Provider switch
        {
            "postgres" => new NpgsqlConnection(ConnectionString),
            "sqlserver" => new SqlConnection(ConnectionString),
            "mysql" => new MySqlConnection(ConnectionString),
            _ => throw new NotSupportedException($"Provider {Provider} is not supported.")
        };
    }
}
