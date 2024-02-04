using Core.Schemas;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Core.Providers;
public class SqlServerDatabaseProvider : IDatabaseProvider
{
    private string _connectionString;

    public SqlServerDatabaseProvider (string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<bool> CheckConnectionAsync ( )
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<TableSchema> GetTableAsync (string tableName)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        DataRowCollection rows = (await connection.GetSchemaAsync("Tables", [null, null, tableName])).Rows;
        if (rows.Count == 0)
            throw new ArgumentException($"Table {tableName} not found", nameof(tableName));
        DataRow row = rows[0];
        TableSchema tableSchema = new() { TableName = row["TABLE_NAME"].ToString()! };
        DataTable columnsSchema = await connection.GetSchemaAsync("Columns", [null, null, tableSchema.TableName]);
        foreach (DataRow columnRow in columnsSchema.Rows)
        {
            string columnName = columnRow["COLUMN_NAME"].ToString()!;
            string dataType = columnRow["DATA_TYPE"].ToString()!;
            bool isNullable = (string) columnRow["IS_NULLABLE"] == "YES";
            tableSchema.Columns.Add(new() { Name = columnName, DataType = dataType, IsNullable = isNullable });
        }
        tableSchema.ForeignKeys = await GetForeignKeys(tableName);
        return tableSchema;
    }
    public async Task<IList<TableSchema>> GetAllTables ( )
    {
        using SqlConnection connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        DataTable schemaTable = await connection.GetSchemaAsync("Tables");
        List<TableSchema> tables = new();
        foreach (DataRow row in schemaTable.Rows)
        {
            TableSchema tableSchema = await GetTableAsync(row["TABLE_NAME"].ToString()!);
            tables.Add(tableSchema);
        }
        return tables;
    }
    public async Task<List<ForeignKeySchema>> GetForeignKeys (string tableName)
    {
        var foreignKeys = new List<ForeignKeySchema>();

        using SqlConnection connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        // Query to retrieve foreign keys information from INFORMATION_SCHEMA
        string query = @"
            SELECT 
                fk.name AS 'Foreign Key Name',
                OBJECT_NAME(fkc.parent_object_id) AS 'Referencing Table',
                COL_NAME(fkc.parent_object_id, fkc.parent_column_id) AS 'Referencing Column',
                OBJECT_NAME(fkc.referenced_object_id) AS 'Referenced Table',
                COL_NAME(fkc.referenced_object_id, fkc.referenced_column_id) AS 'Referenced Column'
            FROM 
                    sys.foreign_keys AS fk
            INNER JOIN 
                    sys.foreign_key_columns AS fkc ON fk.object_id = fkc.constraint_object_id
            WHERE 
                    OBJECT_NAME(fkc.parent_object_id) = @TableName;"
        ;

        using (var command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@TableName", tableName);

            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    foreignKeys.Add(new ForeignKeySchema
                    {
                        TableName = (string) reader["Referencing Table"],
                        ColumnName = (string) reader["Referencing Column"],
                        ReferencedTableName = (string) reader["Referenced Table"],
                        ReferencedColumnName = (string) reader["Referenced Column"]
                    });
                }
            }
        }
        return foreignKeys;
    }

}
