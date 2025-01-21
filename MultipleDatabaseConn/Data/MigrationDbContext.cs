using Microsoft.EntityFrameworkCore;

namespace MultipleDatabaseConn.Data
{
    public class MigrationDbContext(string connectionString) : ApplicationDbContext(connectionString)
    {
        public MigrationDbContext() : this("Host=localhost;Port=5431;Database=migration_db;Username=migration_user;Password=Migration")
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Host=localhost;Port=5431;Database=migration_db;Username=migration_user;Password=Migration");
            }
        }
    }
}
