using Core.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Generators;
public interface ITableGenerator
{
    Task GenerateTables (IDatabaseProvider databaseProvider, string namespaceName, string outputPath, bool generateAllTables, List<string> tablesToGenerate = null!);
}
