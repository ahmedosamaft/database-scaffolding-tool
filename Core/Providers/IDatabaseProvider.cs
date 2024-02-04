using Core.Schemas;
using System.Data;

namespace Core.Providers;

public interface IDatabaseProvider
{
    Task<bool> CheckConnectionAsync(); 
    Task<IList<TableSchema>> GetAllTables ( );
    Task<TableSchema> GetTableAsync (string tableName);
    Task<List<ForeignKeySchema>> GetForeignKeys (string tableName);
}
