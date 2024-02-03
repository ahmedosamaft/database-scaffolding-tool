using System.Data;


namespace Core.Providers;
public class MySqlDatabaseProvider : IDatabaseProvider
{
    private string _connectionString;
    public MySqlDatabaseProvider(string connectionString)
    {
        _connectionString = connectionString;
    }
    public Task<DataTable> GetTables()
    {
        throw new NotImplementedException();
    }
    public Task<DataTable> GetColumns(string tableName)
    {
        throw new NotImplementedException();
    }

}
