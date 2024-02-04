using Core.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Factories;
public class ClassGeneratorFactory : IClassGeneratorFactory
{
    public IClassGenerator Create ( ) => new ClassGenerator();
}
