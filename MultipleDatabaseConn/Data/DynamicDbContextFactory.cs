using Microsoft.EntityFrameworkCore;

namespace MultipleDatabaseConn.Data
{
    public class DynamicDbContextFactory<TContext>(IConnectionStringProvider connectionStringProvider)
        : IDynamicDbContextFactory<TContext>
        where TContext : DynamicDbContext
    {
        public TContext CreateDbContext(string serverName, string databaseName)
        {
            var connectionString = connectionStringProvider.GetConnectionString(serverName, databaseName);
            var dynamicDbContext = (TContext?)Activator.CreateInstance(typeof(TContext), connectionString);
            if (dynamicDbContext is null)
            {
                throw new InvalidOperationException($"Could not create an instance of {typeof(TContext)}");
            }
            dynamicDbContext.Database.EnsureCreated();
            dynamicDbContext.Database.Migrate();
            return dynamicDbContext;
        }
    }
}
