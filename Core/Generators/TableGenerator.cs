using Core.Factories;
using Core.Providers;
using Core.Schemas;
using Humanizer;
using System.Text;

namespace Core.Generators;

public class TableGenerator : ITableGenerator
{
    private readonly IClassGeneratorFactory _classGeneratorFactory;
    private readonly ICrudTemplateGeneratorFactory _crudTemplateGeneratorFactory;

    public TableGenerator (IClassGeneratorFactory classGeneratorFactory, ICrudTemplateGeneratorFactory crudTemplateGeneratorFactory)
    {
        _classGeneratorFactory = classGeneratorFactory ?? throw new ArgumentNullException(nameof(classGeneratorFactory));
        _crudTemplateGeneratorFactory = crudTemplateGeneratorFactory ?? throw new ArgumentNullException(nameof(crudTemplateGeneratorFactory));
    }

    public async Task GenerateTables (IDatabaseProvider databaseProvider, string namespaceName, string outputPath, bool generateAllTables, List<string> tablesToGenerate = null!)
    {
        IList<TableSchema> generatedTables;
        if (generateAllTables)
        {
            generatedTables = await databaseProvider.GetAllTables();
        } else
        {
            try
            {
                generatedTables = new List<TableSchema>();
                foreach (var table in tablesToGenerate)
                {
                    generatedTables.Add(await databaseProvider.GetTableAsync(table));
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error generating tables", ex);
            }
        }

        string modelOutputPath = Path.Combine(outputPath, "Models");
        EnsureDirectoryExists(modelOutputPath);

        string controllerOutputPath = Path.Combine(outputPath, "Controllers");
        EnsureDirectoryExists(controllerOutputPath);

        IClassGenerator classGenerator = _classGeneratorFactory.Create();
        ICrudTemplateGenerator crudTemplateGenerator = _crudTemplateGeneratorFactory.Create();
        List<Task> tasks = new();
        try
        {
            foreach (TableSchema table in generatedTables)
            {
                tasks.Add(Task.Run(async ( ) =>
                {
                    string classFilePath = Path.Combine(modelOutputPath, $"{table.TableName.Singularize()}.cs");
                    await GenerateClassFile(classGenerator, table, namespaceName, classFilePath);
                }));
                tasks.Add(Task.Run(async ( ) =>
                {
                    string controllerFilePath = Path.Combine(controllerOutputPath, $"{table.TableName.Singularize()}Controller.cs");
                    await GenerateControllerFile(crudTemplateGenerator, table, namespaceName, controllerFilePath);
                }));
            }
            await Task.WhenAll(tasks.ToArray());
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error generating files", ex);
        }
    }

    private async Task GenerateClassFile (IClassGenerator classGenerator, TableSchema table, string namespaceName, string filePath)
    {
        string classText = classGenerator.GenerateClass(table, namespaceName);
        using FileStream fileStream = File.Create(filePath);
        await fileStream.WriteAsync(Encoding.UTF8.GetBytes(classText));
    }

    private async Task GenerateControllerFile (ICrudTemplateGenerator crudTemplateGenerator, TableSchema table, string namespaceName, string filePath)
    {
        string crudTemplate = crudTemplateGenerator.GenerateCrudController(table, namespaceName);
        using FileStream fileStream = File.Create(filePath);
        await fileStream.WriteAsync(Encoding.UTF8.GetBytes(crudTemplate));
    }

    private void EnsureDirectoryExists (string directoryPath)
    {
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
    }
}



