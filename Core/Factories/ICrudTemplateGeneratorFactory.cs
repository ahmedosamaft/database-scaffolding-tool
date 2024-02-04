using Core.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Factories;
public interface ICrudTemplateGeneratorFactory
{
    ICrudTemplateGenerator Create ( );
}