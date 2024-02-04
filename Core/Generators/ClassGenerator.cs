using System.Data;
using System.Text;
using Core.Schemas;
using Humanizer;

namespace Core.Generators;
public class ClassGenerator : IClassGenerator
{
    public string GenerateClass(TableSchema table, string @namespace)
    {
        StringBuilder classBuilder = new StringBuilder();
        classBuilder.AppendLine($"namespace {@namespace}.Models;");
        classBuilder.AppendLine($"partial class {table.TableName.Singularize()}");
        classBuilder.AppendLine("{");
        foreach (ColumnSchema column in table.Columns)
        {
            string csharpType = TypeMapper.MapToCSharpType(column.DataType).Name;
            string isNullable = column.IsNullable ? "?" : string.Empty;
            classBuilder.AppendLine($"    public {csharpType}{isNullable} {column.Name} {{ get; set; }}");
        }
        // Generate navigation properties
        foreach (var foreignKey in table.ForeignKeys)
        {
            if (foreignKey.TableName == table.TableName)
                classBuilder.AppendLine($"    public {foreignKey.ReferencedTableName.Singularize()} {foreignKey.ReferencedTableName.Singularize()} {{ get; set; }}");
        }
        classBuilder.AppendLine("}");
        return classBuilder.ToString();
    }
}
