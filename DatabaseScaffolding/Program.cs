using Core.Providers;
using Core.Schemas;
using DatabaseScaffolding.Enums;
using Microsoft.Data.SqlClient;
using Spectre.Console;
using System.Data;
using System.Data.Common;
using System.Text.Json;

public class Program
{

    private static async Task Main (string[] args)
    {
        string connectionString = "Data Source=.;Database=College;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False";
        DBProvider provider = RunMenu();
        // Enter Connection string
        IDatabaseProvider databaseProvider = DatabaseProviderFactory.GetDatabaseProvider(provider, connectionString);
        DataTable tables = await databaseProvider.GetTables();
        List<TableSchema> generatedTables = new();
        foreach (DataRow row in tables.Rows)
        {
            TableSchema table = new() { TableName = row["TABLE_NAME"].ToString()! };
            string tableName = row["TABLE_NAME"].ToString()!;
            DataTable tableSchema = await databaseProvider.GetColumns(tableName);
            foreach (DataRow columnRow in tableSchema.Rows)
            {
                string columnName = columnRow["COLUMN_NAME"].ToString()!;
                string dataType = columnRow["DATA_TYPE"].ToString()!;
                table.Columns.Add(new() { Name = columnName, DataType = dataType });
            }
            generatedTables.Add(table);
        }
        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "TablesSchema.json");
        using FileStream fileStream = File.Create(path);
        await JsonSerializer.SerializeAsync(fileStream,generatedTables);
    }
    public static DBProvider RunMenu ( )
    {
        object[] dBProviders = [.. Enum.GetValues(typeof(DBProvider))];
        string provider = AnsiConsole.Prompt(
                     new SelectionPrompt<string>()
                         .Title("What's your [green]Database Provider[/]?")
                         .PageSize(10)
                         .MoreChoicesText("[grey](Move up and down to reveal more)[/]")
                         .AddChoices(dBProviders.Select(x => x.ToString()!)));
        return (DBProvider) dBProviders.SingleOrDefault(x => x.ToString() == provider)!;
    }

}