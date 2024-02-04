using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Schemas;
public class ForeignKeySchema
{
    public string TableName { get; set; } = null!;
    public string ColumnName { get; set; } = null!;
    public string ReferencedTableName { get; set; } = null!;
    public string ReferencedColumnName { get; set; } = null!;
}