using Core.Providers;
using Core.Providers.Enums;
namespace Core.Factories;
public class DatabaseProviderFactory : IDatabaseProviderFactory
{
    private static async Task<IDatabaseProvider> CheckConnection(IDatabaseProvider provider)
    {
        if (!await provider.CheckConnectionAsync())
            throw new InvalidOperationException("Connection failed");
        return provider;
    }
    /// <summary>
    ///     Get the database provider based on the provider and connection string
    /// </summary>
    /// <param name="provider">Database Provider</param>
    /// <param name="connectionString">Database Connection String</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">If <paramref name="connectionString"/> is wrong</exception>
    public async Task<IDatabaseProvider> GetDatabaseProviderAsync(DBProviderEnum provider, string connectionString) => provider switch
    {
        DBProviderEnum.SqlServer => await CheckConnection(new SqlServerDatabaseProvider(connectionString)),
        DBProviderEnum.MySql => await CheckConnection(new MySqlDatabaseProvider(connectionString)),
        _ => throw new ArgumentException("Unsupported database provider", nameof(provider))
    };
}