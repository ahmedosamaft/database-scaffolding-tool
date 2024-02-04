using System.Text;
using Core.Schemas;
using Humanizer;

namespace Core.Generators;
public class CrudTemplateGenerator : ICrudTemplateGenerator
{
    public string GenerateCrudController (TableSchema table,string @namespace)
    {
        List<ForeignKeySchema> foreignKeys = table.ForeignKeys;
        string entity = table.TableName.Singularize();
        string tableName = table.TableName;
        StringBuilder templateBuilder = new();
        templateBuilder.AppendLine($"using Microsoft.AspNetCore.Mvc;");
        templateBuilder.AppendLine($"using System.Collections.Generic;");
        templateBuilder.AppendLine($"using System.Threading.Tasks;");
        templateBuilder.AppendLine($"using {@namespace}.Repositories;");
        templateBuilder.AppendLine();
        templateBuilder.AppendLine($"namespace {@namespace}.Controllers");
        templateBuilder.AppendLine($"[Route(\"api/[controller]\")]");
        templateBuilder.AppendLine($"[ApiController]");
        templateBuilder.AppendLine($"public class {tableName}Controller : ControllerBase");
        templateBuilder.AppendLine($"{{");

        // Generate CRUD endpoints
        templateBuilder.AppendLine($"    private readonly IRepository<{entity}> _repository;");
        templateBuilder.AppendLine($"");
        templateBuilder.AppendLine($"    public {tableName}Controller(IRepository<{entity}> repository)");
        templateBuilder.AppendLine($"    {{");
        templateBuilder.AppendLine($"        _repository = repository;");
        templateBuilder.AppendLine($"    }}");
        templateBuilder.AppendLine($"");

        // Generate GET endpoint to retrieve all entities
        templateBuilder.AppendLine($"    [HttpGet]");
        templateBuilder.AppendLine($"    public async Task<ActionResult<IEnumerable<{entity}>>> GetAll()");
        templateBuilder.AppendLine($"    {{");
        templateBuilder.AppendLine($"        var {tableName.ToLower()} = await _repository.GetAll();");
        templateBuilder.AppendLine($"        return Ok({tableName.ToLower()});");
        templateBuilder.AppendLine($"    }}");
        templateBuilder.AppendLine($"");

        // Generate GET endpoint to retrieve a single entity by ID
        templateBuilder.AppendLine($"    [HttpGet(\"/{{id}}\")]");
        templateBuilder.AppendLine($"    public async Task<ActionResult<{entity}>> GetById(int id)");
        templateBuilder.AppendLine($"    {{");
        templateBuilder.AppendLine($"        var {entity.ToLower()} = await _repository.GetById(id);");
        templateBuilder.AppendLine($"        if ({entity.ToLower()} is null)");
        templateBuilder.AppendLine($"        {{");
        templateBuilder.AppendLine($"            return NotFound();");
        templateBuilder.AppendLine($"        }}");
        templateBuilder.AppendLine($"        return Ok({entity.ToLower()});");
        templateBuilder.AppendLine($"    }}");
        templateBuilder.AppendLine($"");

        // Generate POST endpoint to create a new entity
        templateBuilder.AppendLine($"    [HttpPost]");
        templateBuilder.AppendLine($"    public async Task<ActionResult<{entity}>> Create({entity} {entity.ToLower()})");
        templateBuilder.AppendLine($"    {{");
        templateBuilder.AppendLine($"        await _repository.Create({entity.ToLower()});");
        templateBuilder.AppendLine($"        return CreatedAtAction(nameof(GetById), new {{ id = {entity.ToLower()}.Id }}, {entity.ToLower()});");
        templateBuilder.AppendLine($"    }}");
        templateBuilder.AppendLine($"");

        // Generate PUT endpoint to update an existing entity
        templateBuilder.AppendLine($"    [HttpPut(\"/{{id}}\")]");
        templateBuilder.AppendLine($"    public async Task<IActionResult> Update(int id, {entity} {entity.ToLower()})");
        templateBuilder.AppendLine($"    {{");
        templateBuilder.AppendLine($"        if (id != {entity.ToLower()}.Id)");
        templateBuilder.AppendLine($"        {{");
        templateBuilder.AppendLine($"            return BadRequest();");
        templateBuilder.AppendLine($"        }}");
        templateBuilder.AppendLine($"        await _repository.Update({entity.ToLower()});");
        templateBuilder.AppendLine($"        return NoContent();");
        templateBuilder.AppendLine($"    }}");
        templateBuilder.AppendLine($"");

        // Generate DELETE endpoint to delete an entity by ID
        templateBuilder.AppendLine($"    [HttpDelete(\"/{{id}}\")]");
        templateBuilder.AppendLine($"    public async Task<IActionResult> Delete(int id)");
        templateBuilder.AppendLine($"    {{");
        templateBuilder.AppendLine($"        var {entity.ToLower()} = await _repository.GetById(id);");
        templateBuilder.AppendLine($"        if ({entity.ToLower()} is null)");
        templateBuilder.AppendLine($"        {{");
        templateBuilder.AppendLine($"            return NotFound();");
        templateBuilder.AppendLine($"        }}");
        templateBuilder.AppendLine($"        await _repository.Delete(id);");
        templateBuilder.AppendLine($"        return NoContent();");
        templateBuilder.AppendLine($"    }}");
        templateBuilder.AppendLine($"");

        // Generate additional endpoints for foreign key relationships
        foreach (var foreignKey in foreignKeys)
        {
            templateBuilder.AppendLine($"    // Endpoint to retrieve related {foreignKey.ReferencedTableName.Singularize()}");
            templateBuilder.AppendLine($"    [HttpGet(\"/{{id}}/{foreignKey.ReferencedTableName.Singularize().ToLower()}\")]");
            templateBuilder.AppendLine($"    public async Task<ActionResult<IEnumerable<{foreignKey.ReferencedTableName.Singularize()}>>>> Get{foreignKey.ReferencedTableName.Singularize()}By{entity}Id(int id)");
            templateBuilder.AppendLine($"    {{");
            templateBuilder.AppendLine($"        // Implement logic to retrieve related {foreignKey.ReferencedTableName.Singularize()} entities by {entity} ID");
            templateBuilder.AppendLine($"    }}");
            templateBuilder.AppendLine($"");
        }

        templateBuilder.AppendLine($"}}");

        return templateBuilder.ToString();
    }
}
