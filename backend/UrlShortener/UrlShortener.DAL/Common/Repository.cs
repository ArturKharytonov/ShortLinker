using Microsoft.EntityFrameworkCore;
using UrlShortener.DAL.Common.Interfaces;
using UrlShortener.DAL.DbContext;
using UrlShortener.DAL.Entities.Common;

namespace UrlShortener.DAL.Common;

public class Repository<TEntity, TId> : IRepository<TEntity, TId> 
    where TEntity : class, IDbEntity<TId>
{
    private readonly UrlShorterContext _context;
    public Repository(UrlShorterContext context)
    {
        _context = context;
    }

    public async Task<TEntity?> GetByIdAsync(TId id)
    {
        return await _context.Set<TEntity>().FindAsync(id);
    }
    public async Task CreateAsync(TEntity entity)
    {
        await _context.Set<TEntity>().AddAsync(entity);
    }
    public async Task DeleteAsync(TId id)
    {
        await _context.Set<TEntity>()
            .Where(entity => entity.Id!.Equals(id))
            .ExecuteDeleteAsync();
    }
    public  IQueryable<TEntity> GetAllAsQueryable()
    {
        return _context.Set<TEntity>().AsQueryable();
    }
}