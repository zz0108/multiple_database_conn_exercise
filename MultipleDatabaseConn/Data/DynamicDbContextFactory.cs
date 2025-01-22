using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Npgsql;

namespace MultipleDatabaseConn.Data
{
    public class DynamicDbContextFactory<TContext>(
        IConnectionStringProvider connectionStringProvider,
        IOptions<Models.DbContextOptions> options)
        : IDynamicDbContextFactory<TContext>
        where TContext : DynamicDbContext
    {
        public TContext CreateDbContext(string serverName, string databaseName)
        {
            var baseConnectionString = connectionStringProvider.GetConnectionString(serverName, databaseName);
            var builder = new NpgsqlConnectionStringBuilder(baseConnectionString)
            {
                MaxPoolSize = options.Value.MaxPoolSize,
                MinPoolSize = options.Value.MinPoolSize,
                Timeout = options.Value.CommandTimeout
            };
            var connectionString = builder.ConnectionString;
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
