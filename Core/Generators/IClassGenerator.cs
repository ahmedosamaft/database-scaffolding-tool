using Core.Schemas;

namespace Core.Generators;

public interface IClassGenerator
{
    string GenerateClass(TableSchema table, string @namespace);
}