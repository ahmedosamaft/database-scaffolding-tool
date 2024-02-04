using Core.Providers;
using Core.Providers.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Factories;
public interface IDatabaseProviderFactory
{
    Task<IDatabaseProvider> GetDatabaseProviderAsync (DBProviderEnum provider, string connectionString);
}
