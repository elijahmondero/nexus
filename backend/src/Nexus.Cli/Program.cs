using System.Diagnostics;
using Spectre.Console;

namespace Nexus.Cli;

class Program
{
    static void Main(string[] args)
    {
        AnsiConsole.Write(
            new FigletText("NEXUS CLI")
                .Color(Color.Blue));

        if (args.Length == 0)
        {
            ShowHelp();
            return;
        }

        var command = args[0].ToLower();
        switch (command)
        {
            case "add-package":
                HandleAddPackage(args.Skip(1).ToArray());
                break;
            case "add-feature":
                HandleAddFeature(args.Skip(1).ToArray());
                break;
            case "db-switch":
                HandleDbSwitch(args.Skip(1).ToArray());
                break;
            default:
                AnsiConsole.MarkupLine("[red]Unknown command.[/]");
                ShowHelp();
                break;
        }
    }

    static void ShowHelp()
    {
        var table = new Table();
        table.AddColumn("Command");
        table.AddColumn("Description");
        table.AddRow("add-package <name>", "Adds a NuGet package to the Api project.");
        table.AddRow("add-feature <name>", "Scaffolds a new backend feature slice.");
        table.AddRow("db-switch <provider>", "Switches the database provider (postgres|sqlserver|mysql).");
        AnsiConsole.Write(table);
    }

    static void HandleAddPackage(string[] args)
    {
        if (args.Length == 0)
        {
            var packageName = AnsiConsole.Ask<string>("Enter the [green]NuGet package name[/]:");
            RunDotnetCommand($"add src/Nexus.Api/Nexus.Api.csproj package {packageName}");
        }
        else
        {
            RunDotnetCommand($"add src/Nexus.Api/Nexus.Api.csproj package {args[0]}");
        }
    }

    static void HandleAddFeature(string[] args)
    {
        var featureName = args.Length > 0 ? args[0] : AnsiConsole.Ask<string>("Enter the [green]Feature name[/] (e.g. Products):");
        
        AnsiConsole.Status()
            .Start($"Scaffolding [bold blue]{featureName}[/]...", ctx => 
            {
                var featurePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "src", "Nexus.Api", "Features", featureName);
                Directory.CreateDirectory(featurePath);
                
                // Feature scaffolding logic
                File.WriteAllText(Path.Combine(featurePath, $"{featureName}Controller.cs"), $"// Generated {featureName}Controller");
                
                Thread.Sleep(1000); // Simulate work
                AnsiConsole.MarkupLine($"[green]✔[/] Feature [bold]{featureName}[/] created at {featurePath}");
            });
    }

    static void HandleDbSwitch(string[] args)
    {
        var provider = args.Length > 0 ? args[0] : AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Select a [green]Database Provider[/]")
                .AddChoices(new[] { "postgres", "sqlserver", "mysql" }));

        AnsiConsole.MarkupLine($"Configuring environment for [bold blue]{provider}[/]...");
        AnsiConsole.MarkupLine("[yellow]Please ensure your .env file is updated with the matching connection string.[/]");
    }

    static void RunDotnetCommand(string args)
    {
        AnsiConsole.MarkupLine($"[grey]Executing: dotnet {args}[/]");
        
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = args,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = Path.Combine(Directory.GetCurrentDirectory(), "..")
            }
        };

        process.Start();
        string output = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();
        process.WaitForExit();

        if (process.ExitCode == 0)
        {
            AnsiConsole.MarkupLine("[green]✔ Command executed successfully.[/]");
            AnsiConsole.WriteLine(output);
        }
        else
        {
            AnsiConsole.MarkupLine("[red]✘ Command failed.[/]");
            AnsiConsole.WriteLine(error);
        }
    }
}
