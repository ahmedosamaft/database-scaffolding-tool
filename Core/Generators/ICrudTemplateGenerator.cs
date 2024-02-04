using Core.Schemas;

namespace Core.Generators;

public interface ICrudTemplateGenerator
{
    string GenerateCrudController (TableSchema table, string @namespace);
}