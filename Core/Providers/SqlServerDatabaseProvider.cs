using Microsoft.Data.SqlClient;
using System.Data;

namespace Core.Providers;
public class SqlServerDatabaseProvider : IDatabaseProvider
{
    private string _connectionString;
    // Implement methods to retrieve schema from SQL Server
    public SqlServerDatabaseProvider(string connectionString)
    {
        _connectionString = connectionString;
    }
    public async Task<DataTable> GetTables()
    {
        using SqlConnection connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        DataTable schemaTable = await connection.GetSchemaAsync("Tables");
        return schemaTable;
    }

    public async Task<DataTable> GetColumns(string tableName)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        DataTable columnsSchema = await connection.GetSchemaAsync("Columns", [null, null, tableName]);
        return columnsSchema;
    }
}
