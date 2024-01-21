using UrlShortener.DAL.Common;
using UrlShortener.DAL.Common.Interfaces;
using UrlShortener.DAL.DbContext;
using UrlShortener.DAL.Entities.Common;
using UrlShortener.DAL.UnitOfWork.Interfaces;

namespace UrlShortener.DAL.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly UrlShorterContext _context;
    public UnitOfWork(UrlShorterContext context)
        => _context = context;

    public IRepository<TEntity, TId>? GetRepository<TEntity, TId>()
        where TEntity : class, IDbEntity<TId>
    {
        return new Repository<TEntity, TId>(_context);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}