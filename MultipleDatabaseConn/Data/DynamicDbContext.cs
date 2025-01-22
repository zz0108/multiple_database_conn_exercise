using Microsoft.EntityFrameworkCore;

namespace MultipleDatabaseConn.Data
{
    public class DynamicDbContext : DbContext
    {
        protected DynamicDbContext() : base()
        {
        }

        protected DynamicDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
