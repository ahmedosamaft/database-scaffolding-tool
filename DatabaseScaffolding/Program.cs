using Core.Factories;
using Core.Generators;
using Core.Providers;
using Core.Providers.Enums;
using Spectre.Console;
using System.Data;
using System.Text.RegularExpressions;

public class Program
{
    static async Task Main (string[] args)
    {
        AnsiConsole.Write(new FigletText("Database Scaffolding Tool").Color(Color.Red1).Centered());
        AnsiConsole.Status()
                         .Start("Loading", ctx =>
                         {
                             ctx.Spinner(Spinner.Known.Dots2);
                             ctx.SpinnerStyle(Style.Parse("red"));
                             Thread.Sleep(2000);
                         });
        AnsiConsole.WriteLine();
        Console.Clear();
        AnsiConsole.MarkupLine("Welcome to the [bold yellow]Database Scaffolding Tool[/]!");
        AnsiConsole.WriteLine();
        // Step 1: Ask for the database provider
        DBProviderEnum provider = ShowProviders();

        // Step 2: Ask for the namespace
        string namespaceName;
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Please enter the namespace:");
            namespaceName = Console.ReadLine()!;
            if (Regex.IsMatch(namespaceName, @"^[a-zA-Z_][a-zA-Z0-9_]*$"))
                break;
            else
            {
                AnsiConsole.Status()
                         .Start("Invalid namespace. Please enter a valid namespace.", ctx =>
                         {
                             ctx.Spinner(Spinner.Known.Dots2);
                             ctx.SpinnerStyle(Style.Parse("red"));
                             Thread.Sleep(2000);
                         });
            }
        }
        // Step 3: Ask for the connection string


        // Step 4: Check Connection String
        IDatabaseProviderFactory databaseProviderFactory = new DatabaseProviderFactory();
        IDatabaseProvider databaseProvider;
        while (true)
        {
            try
            {
                Console.Clear();
                Console.WriteLine("Please enter the connection string:");
                string connectionString = Console.ReadLine()!;
                databaseProvider = await databaseProviderFactory.GetDatabaseProviderAsync(provider: provider, connectionString: connectionString);
                break;
            }
            catch (Exception)
            {

                AnsiConsole.Status()
                        .Start("Connection failed. Please check the connection string", ctx =>
                        {
                            AnsiConsole.MarkupLine("[bold grey74] LOG: [/] Cleaning last connection");
                            ctx.Spinner(Spinner.Known.Toggle7);
                            ctx.SpinnerStyle(Style.Parse("red"));
                            Thread.Sleep(2000);
                            AnsiConsole.MarkupLine("[bold blue] Command: [/]Enter connection string again...");
                            ctx.Spinner(Spinner.Known.Dots2);
                            ctx.SpinnerStyle(Style.Parse("blue"));
                            Thread.Sleep(1000);
                        });
            }
        }
        //Data Source=AHMED-OSAMA;Database=College;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False
        // Step 5: Ask for the output path for controllers and models
        string outputPath;
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Please enter the output path:");
            outputPath = Console.ReadLine()!;
            if (Directory.Exists(outputPath))
                break;
            else
            {
                AnsiConsole.Status()
                         .Start("Invalid path. Please enter a valid path.", ctx =>
                         {
                             ctx.Spinner(Spinner.Known.Dots2);
                             ctx.SpinnerStyle(Style.Parse("red"));
                             Thread.Sleep(2000);
                         });
            }
        }

        // Step 6: Ask if the user wants to generate specific tables or all tables
        Console.Clear();
        bool generateAllTables = AskGenerateAllTables();

        List<string> tablesToGenerate = new List<string>();

        if (!generateAllTables)
        {
            // Step 6: Ask for specific tables to generate
            Console.WriteLine("Please enter the names of the tables you want to generate (comma-separated):");
            string tablesInput = Console.ReadLine()!;
            tablesToGenerate.AddRange(tablesInput.Split(',').Select(x => x.Trim()));
        }
        Console.Clear();
        //await GenerateTables(databaseProvider: databaseProvider, namespaceName: namespaceName, outputPath: outputPath, generateAllTables: generateAllTables, tablesToGenerate);
        ITableGenerator tableGenerator = new TableGenerator(new ClassGeneratorFactory(), new CrudTemplateGeneratorFactory());
        try
        {
            await tableGenerator.GenerateTables(databaseProvider, namespaceName, outputPath, generateAllTables, tablesToGenerate);
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
            if (ex.InnerException != null)
            {
                AnsiConsole.MarkupLine($"[red]    {ex.InnerException.Message}[/]");
            }
        }
    }
    public static DBProviderEnum ShowProviders ( )
    {
        object[] dBProviders = [.. Enum.GetValues(typeof(DBProviderEnum))];
        string provider = AnsiConsole.Prompt(
                     new SelectionPrompt<string>()
                         .Title("What's your [green]Database Provider[/]?")
                         .PageSize(10)
                         .MoreChoicesText("[grey](Move up and down to reveal more)[/]")
                         .AddChoices(dBProviders.Select(x => x.ToString()!)));
        return (DBProviderEnum) dBProviders.SingleOrDefault(x => x.ToString() == provider)!;
    }

    public static bool AskGenerateAllTables ( )
    {
        string[] choices = ["specific", "all"];
        string choice = AnsiConsole.Prompt(
                     new SelectionPrompt<string>()
                         .Title("Do you want to generate specific tables or all tables?")
                         .AddChoices(choices));
        return choice == "all";
    }

}