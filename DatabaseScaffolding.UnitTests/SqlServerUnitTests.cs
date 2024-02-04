using Core.Factories;
using Core.Providers;
using Core.Providers.Enums;
using Core.Schemas;

namespace DatabaseScaffolding.UnitTests;

public class SqlServerUnitTests
{
    [Fact]
    public async Task GetTableTest ( )
    {
        #region Arrange
        string connectionString = "Data Source=.;Database=College;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False";
        IDatabaseProvider databaseProvider = await DatabaseProviderFactory.GetDatabaseProviderAsync(DBProviderEnum.SqlServer, connectionString);
        List<ForeignKeySchema> foreignKeys = [new() { TableName = "Students", ColumnName = "DepartmentId", ReferencedTableName = "Departments", ReferencedColumnName = "Id" }, new() { TableName = "Students", ColumnName = "Id", ReferencedTableName = "Users", ReferencedColumnName = "Id" }];
        List<ColumnSchema> columns = [new() { Name = "Id", DataType = "varchar" }, new() { Name = "Gpa", DataType = "decimal" }, new() { Name = "DepartmentId", DataType = "varchar" }];
        TableSchema actual = new() { TableName = "Students", ForeignKeys = foreignKeys, Columns = columns };
        #endregion
        #region Act
        TableSchema table = await databaseProvider.GetTableAsync("Students");
        #endregion
        #region Assert
        Assert.NotNull(table);
        Assert.Equal(actual.TableName, table.TableName);
        Assert.Equal(actual.Columns.Count, table.Columns.Count);
        Assert.Equal(actual.ForeignKeys.Count, table.ForeignKeys.Count);
        for (int i = 0;i < actual.Columns.Count;i++)
        {
            Assert.Equal(actual.Columns[i].Name, table.Columns[i].Name);
            Assert.Equal(actual.Columns[i].DataType, table.Columns[i].DataType);
        }
        for (int i = 0;i < actual.ForeignKeys.Count;i++)
        {
            Assert.Equal(actual.ForeignKeys[i].TableName, table.ForeignKeys[i].TableName);
            Assert.Equal(actual.ForeignKeys[i].ColumnName, table.ForeignKeys[i].ColumnName);
            Assert.Equal(actual.ForeignKeys[i].ReferencedTableName, table.ForeignKeys[i].ReferencedTableName);
            Assert.Equal(actual.ForeignKeys[i].ReferencedColumnName, table.ForeignKeys[i].ReferencedColumnName);
        }
        #endregion
    }
    [Fact]
    public async Task GetNonExistingTableTest ( )
    {
        #region Arrange
        string connectionString = "Data Source=.;Database=College;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False";
        #endregion
        #region Act
        IDatabaseProvider databaseProvider = await DatabaseProviderFactory.GetDatabaseProviderAsync(DBProviderEnum.SqlServer, connectionString);
        #endregion
        #region Assert
        await Assert.ThrowsAsync<ArgumentException>(async ( ) => await databaseProvider.GetTableAsync("WrongTable!"));
        #endregion
    }
    [Fact]
    public async Task WrongConnectionStringTest ( )
    {
        #region Arrange
        string invalidConnectionString = ".";
        #endregion
        #region Act and Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async ( ) => await DatabaseProviderFactory.GetDatabaseProviderAsync(DBProviderEnum.SqlServer, invalidConnectionString));
        #endregion
    }
    [Fact]
    public async Task GetAllTablesTest ( )
    {
        #region Arrange
        string connectionString = "Data Source=.;Database=College;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False";
        IDatabaseProvider databaseProvider = await DatabaseProviderFactory.GetDatabaseProviderAsync(DBProviderEnum.SqlServer, connectionString);
        #endregion
        #region Act
        IList<TableSchema> tables = await databaseProvider.GetAllTables();
        #endregion
        #region Assert
        Assert.NotNull(tables);
        Assert.NotEmpty(tables);
        #endregion
    }
    [Fact]
    public async Task GetForeignKeysTest ( )
    {
        #region Arrange
        string connectionString = "Data Source=.;Database=College;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False";
        IDatabaseProvider databaseProvider = await DatabaseProviderFactory.GetDatabaseProviderAsync(DBProviderEnum.SqlServer, connectionString);
        List<ForeignKeySchema> actual = [new() { TableName = "Students", ColumnName = "DepartmentId", ReferencedTableName = "Departments", ReferencedColumnName = "Id" }, new() { TableName = "Students", ColumnName = "Id", ReferencedTableName = "Users", ReferencedColumnName = "Id" }];
        #endregion
        #region Act
        List<ForeignKeySchema> foreignKeys = await databaseProvider.GetForeignKeys("Students");
        #endregion
        #region Assert
        Assert.NotNull(foreignKeys);
        Assert.Equal(actual.Count, foreignKeys.Count);
        for (int i = 0;i < actual.Count;i++)
        {
            Assert.Equal(actual[i].TableName, foreignKeys[i].TableName);
            Assert.Equal(actual[i].ColumnName, foreignKeys[i].ColumnName);
            Assert.Equal(actual[i].ReferencedTableName, foreignKeys[i].ReferencedTableName);
            Assert.Equal(actual[i].ReferencedColumnName, foreignKeys[i].ReferencedColumnName);
        }
        #endregion
    }
}