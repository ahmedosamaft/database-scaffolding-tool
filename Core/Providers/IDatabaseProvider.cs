using System.Data;

namespace Core.Providers;

public interface IDatabaseProvider
{
    Task<DataTable> GetTables();
    Task<DataTable> GetColumns(string tableName);
}
