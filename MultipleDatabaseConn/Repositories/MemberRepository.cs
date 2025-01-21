using Microsoft.Extensions.Caching.Memory;
using MultipleDatabaseConn.Data;
using MultipleDatabaseConn.Models;

namespace MultipleDatabaseConn.Repositories
{
    public class MemberRepository(IDynamicDbContextFactory<ApplicationDbContext> dynamicDbContextFactory, IMemoryCache cache,ILogger<BaseRepository<Member>> logger) : BaseRepository<Member>(dynamicDbContextFactory,cache, logger), IMemberRepository
    {
    }
}
