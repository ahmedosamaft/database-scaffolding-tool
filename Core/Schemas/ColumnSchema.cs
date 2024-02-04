
namespace Core.Schemas;

public class ColumnSchema
{
    public string Name { get; set; } = null!;
    public string DataType { get; set; } = null!;
    public bool IsNullable { get; set; }
}
