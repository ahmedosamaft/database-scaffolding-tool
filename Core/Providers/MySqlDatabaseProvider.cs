using Core.Schemas;
using System.Data;

namespace Core.Providers;
public class MySqlDatabaseProvider (string connectionString) : IDatabaseProvider
{
    private string _connectionString = connectionString;

    public Task<bool> CheckConnectionAsync ( )
    {
        throw new NotImplementedException();
    }

    public Task<IList<TableSchema>> GetAllTables ( )
    {
        throw new NotImplementedException();
    }

    public Task<List<ForeignKeySchema>> GetForeignKeys (string tableName)
    {
        throw new NotImplementedException();
    }

    public Task<TableSchema> GetTableAsync (string tableName)
    {
        throw new NotImplementedException();
    }
}
