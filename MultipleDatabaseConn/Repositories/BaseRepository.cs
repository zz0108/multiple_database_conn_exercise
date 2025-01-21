using MultipleDatabaseConn.Data;
using MultipleDatabaseConn.Enum;
using MultipleDatabaseConn.Models;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace MultipleDatabaseConn.Repositories
{
    public class BaseRepository<T>(IDynamicDbContextFactory<ApplicationDbContext> dynamicDbContextFactory) : IRepository<T> where T : class, IEntity
    {
        public async Task Create(T entity)
        {
            var serverName = GetServerName(entity.Id);
            var connString = GetConnString(entity.Id);
            await using var dbContext = dynamicDbContextFactory.CreateDbContext(serverName, connString);
            await dbContext.Set<T>().AddAsync(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task MultipleCreate(List<T> entities)
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

        public async Task Update(T entity)
        {
            var serverName = GetServerName(entity.Id);
            var connString = GetConnString(entity.Id);
            await using var dbContext = dynamicDbContextFactory.CreateDbContext(serverName, connString);
            dbContext.Set<T>().Update(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task Delete(T entity)
        {
            var serverName = GetServerName(entity.Id);
            var connString = GetConnString(entity.Id);
            await using var dbContext = dynamicDbContextFactory.CreateDbContext(serverName, connString);
            dbContext.Set<T>().Remove(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task<T?> GetById(Guid id)
        {
            var serverName = GetServerName(id);
            var connString = GetConnString(id);
            await using var dbContext = dynamicDbContextFactory.CreateDbContext(serverName, connString);
            return await dbContext.Set<T>().FindAsync(id);
        }

        public async Task<PagedResult<T>> GetAll(int pageNumber, int pageSize)
        {
            var allEntities = new List<T>();
            foreach (var serverName in typeof(ServerNames).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.IgnoreCase)
                         .Select(f => f.GetValue(null) as string))
            {
                if (serverName is null)
                    continue;

                var connString = GetConnString(Guid.NewGuid());
                await using var dbContext = dynamicDbContextFactory.CreateDbContext(serverName, connString);
                var entities = await dbContext.Set<T>().ToListAsync();
                if (entities.Any())
                    allEntities.AddRange(entities);
            }

            var totalCount = allEntities.Count;
            var items = allEntities
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedResult<T>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
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

        private string GetLastHex(Guid id)
        {
            return id.ToString("N")[^1..].ToUpper();
        }
    }
}
