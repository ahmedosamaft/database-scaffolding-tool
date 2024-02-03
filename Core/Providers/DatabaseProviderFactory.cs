using DatabaseScaffolding.Enums;
namespace Core.Providers;
public class DatabaseProviderFactory
{
    public static IDatabaseProvider GetDatabaseProvider(DBProvider provider, string connectionString) => provider switch
    {
        DBProvider.SqlServer => new SqlServerDatabaseProvider(connectionString),
        DBProvider.MySql => new MySqlDatabaseProvider(connectionString),
        _ => throw new ArgumentException("Unsupported database provider", nameof(provider))
    };


}