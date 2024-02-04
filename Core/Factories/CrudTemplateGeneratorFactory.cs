using Core.Generators;
namespace Core.Factories;
public class CrudTemplateGeneratorFactory : ICrudTemplateGeneratorFactory
{
    public ICrudTemplateGenerator Create ( ) => new CrudTemplateGenerator();
}
