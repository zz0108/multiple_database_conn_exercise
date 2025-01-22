using MultipleDatabaseConn.Data;
using MultipleDatabaseConn.Enum;
using MultipleDatabaseConn.Models;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace MultipleDatabaseConn.Repositories
{
    public class BaseRepository<T>(IDynamicDbContextFactory<ApplicationDbContext> dynamicDbContextFactory, IMemoryCache cache, ILogger<BaseRepository<T>> logger) : IRepository<T> where T : class, IEntity
    {
        private readonly string _typeName = typeof(T).Name;
        public async Task CreateAsync(T entity)
        {
            var serverName = GetServerName(entity.Id);
            var connString = GetConnString(entity.Id);
            await using var dbContext = dynamicDbContextFactory.CreateDbContext(serverName, connString);
            await dbContext.Set<T>().AddAsync(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task MultipleCreateAsync(List<T> entities)
        {
            var entityGroups = entities.GroupBy(g => GetLastHex(g.Id));
            foreach (var entityGroup in entityGroups)
            {
                var serverName = GetServerName(entityGroup.First().Id);
                var connString = GetConnString(entityGroup.First().Id);
                await using var dbContext = dynamicDbContextFactory.CreateDbContext(serverName, connString);
                await dbContext.Set<T>().AddRangeAsync(entityGroup);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(T entity)
        {
            var serverName = GetServerName(entity.Id);
            var connString = GetConnString(entity.Id);
            await using var dbContext = dynamicDbContextFactory.CreateDbContext(serverName, connString);
            dbContext.Set<T>().Update(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            var serverName = GetServerName(entity.Id);
            var connString = GetConnString(entity.Id);
            await using var dbContext = dynamicDbContextFactory.CreateDbContext(serverName, connString);
            dbContext.Set<T>().Remove(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            var cacheKey = GetCacheKey("GetById", id);

            return await cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.SetSlidingExpiration(TimeSpan.FromMinutes(30));
                var serverName = GetServerName(id);
                var connString = GetConnString(id);
                await using var dbContext = dynamicDbContextFactory.CreateDbContext(serverName, connString);
                return await dbContext.Set<T>().AsNoTracking().FirstOrDefaultAsync(f => f.Id == id);
            });
        }

        public async Task<PagedResult<T>?> GetAllAsync(int pageNumber, int pageSize)
        {
            var cacheKey = GetCacheKey("GetAll", pageNumber, pageSize);

            return await cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                var databaseCounts = new List<(string Name, string Value, string ConnString, int Count, int StartIndex)>();
                var totalCount = 0;

                entry.SetSlidingExpiration(TimeSpan.FromMinutes(5));
                try
                {
                    foreach (var serverName in typeof(ServerNames)
                        .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.IgnoreCase)
                        .Select(f => new { Name = f.Name, Value = f.GetValue(null) as string }))
                    {
                        if (serverName.Value is null)
                            continue;
                        var connString = GetGetConnString(serverName.Name);
                        await using var dbContext = dynamicDbContextFactory.CreateDbContext(serverName.Value, connString);

                        var count = await dbContext.Set<T>().AsNoTracking().CountAsync();
                        databaseCounts.Add((serverName.Name, serverName.Value, connString, count, totalCount));
                        totalCount += count;
                    }

                    var startIndex = (pageNumber - 1) * pageSize;
                    var items = new List<T>();

                    foreach (var db in databaseCounts)
                    {
                        if (startIndex >= db.StartIndex + db.Count ||
                            startIndex + pageSize <= db.StartIndex)
                        {
                            continue;
                        }

                        var localSkip = Math.Max(0, startIndex - db.StartIndex);

                        var localTake = Math.Min(
                            pageSize - items.Count,
                            db.Count - localSkip
                        );

                        if (localTake <= 0) continue;

                        await using var dbContext = dynamicDbContextFactory.CreateDbContext(db.Value, db.ConnString);
                        var currentItems = await dbContext.Set<T>()
                            .AsNoTracking()
                            .Skip(localSkip)
                            .Take(localTake)
                            .ToListAsync();

                        items.AddRange(currentItems);

                        if (items.Count >= pageSize)
                            break;
                    }

                    return new PagedResult<T>
                    {
                        Items = items,
                        TotalCount = totalCount,
                        PageNumber = pageNumber,
                        PageSize = pageSize
                    };
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "取得所有資料時發生錯誤");
                    return new PagedResult<T>
                    {
                        Items = [],
                        TotalCount = totalCount,
                        PageNumber = pageNumber,
                        PageSize = pageSize
                    }; ;
                }
            });
        }

        private string GetCacheKey(string operation, params object[] parameters)
        {
            return $"{_typeName}_{operation}_{string.Join("_", parameters)}";
        }

        private string GetServerName(Guid id)
        {
            var serverConstName = $"X0{GetLastHex(id)}";

            var serverName = typeof(ServerNames)
                .GetField(serverConstName, BindingFlags.Public | BindingFlags.Static | BindingFlags.IgnoreCase)
                ?.GetValue(null) as string;

            return serverName ?? throw new InvalidOperationException("Server name cannot be null");
        }

        private string GetConnString(Guid id)
        {
            var connStringName = $"X0{GetLastHex(id)}";

            var connString = typeof(Conn)
                .GetField(connStringName, BindingFlags.Public | BindingFlags.Static | BindingFlags.IgnoreCase)
                ?.GetValue(null) as string;

            return connString ?? throw new InvalidOperationException("Conn String cannot be null");
        }

        private string GetGetConnString(string connStringName)
        {
            var connString = typeof(Conn)
                .GetField(connStringName, BindingFlags.Public | BindingFlags.Static | BindingFlags.IgnoreCase)
                ?.GetValue(null) as string;

            return connString ?? throw new InvalidOperationException("Conn String cannot be null");
        }

        private string GetLastHex(Guid id)
        {
            return id.ToString("N")[^1..].ToUpper();
        }
    }
}
