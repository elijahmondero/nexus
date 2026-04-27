using System.Reflection;
using DbUp;
using DbUp.Engine;

namespace Nexus.Api.Data.Migrations;

public static class MigrationRunner
{
    public static void Run(string connectionString, string provider)
    {
        var upgradeEngineBuilder = provider switch
        {
            "postgres" => DeployChanges.To.PostgresqlDatabase(connectionString),
            "sqlserver" => DeployChanges.To.SqlDatabase(connectionString),
            "mysql" => DeployChanges.To.MySqlDatabase(connectionString),
            _ => throw new NotSupportedException($"Provider {provider} is not supported by DbUp.")
        };

        var upgradeEngine = upgradeEngineBuilder
            .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(), s => s.Contains("Data.Migrations.Scripts"))
            .LogToConsole()
            .Build();

        var result = upgradeEngine.PerformUpgrade();

        if (!result.Successful)
        {
            Console.WriteLine(result.Error);
        }
    }
}
