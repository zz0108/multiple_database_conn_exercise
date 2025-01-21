using MultipleDatabaseConn.Data;
using MultipleDatabaseConn.Models;

namespace MultipleDatabaseConn.Repositories
{
    public class MemberRepository(IDynamicDbContextFactory<ApplicationDbContext> dynamicDbContextFactory) : BaseRepository<Member>(dynamicDbContextFactory), IMemberRepository
    {
    }
}
