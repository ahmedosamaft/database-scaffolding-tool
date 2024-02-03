namespace Core;
public class TypeMapper
{
    public static Type MapToCSharpType (string dbType) => dbType switch
    {
        "int" => typeof(int),
        "bigint" => typeof(long),
        "smallint" => typeof(short),
        "tinyint" => typeof(byte),
        "bit" => typeof(bool),
        "decimal" or "numeric" or "money" or "smallmoney" => typeof(decimal),
        "float" => typeof(double),
        "real" => typeof(float),
        "datetime" or "date" or "datetime2" or "datetimeoffset" => typeof(DateTime),
        "time" => typeof(TimeSpan),
        "char" or "varchar" or "text" or "nchar" or "nvarchar" or "ntext" => typeof(string),
        "varbinary" => typeof(byte[]),
        _ => throw new ArgumentException($"Unsupported database type: {dbType}", nameof(dbType)),
    };
}
