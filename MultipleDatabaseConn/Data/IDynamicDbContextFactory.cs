namespace MultipleDatabaseConn.Data
{
    public interface IDynamicDbContextFactory<out TContext>
        where TContext : DynamicDbContext
    {
        TContext CreateDbContext(string serverName, string databaseName);
    }
}
