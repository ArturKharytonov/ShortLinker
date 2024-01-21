using UrlShortener.DAL.Common.Interfaces;
using UrlShortener.DAL.Entities.Common;

namespace UrlShortener.DAL.UnitOfWork.Interfaces;

public interface IUnitOfWork
{
    IRepository<TEntity, TId>? GetRepository<TEntity, TId>()
        where TEntity : class, IDbEntity<TId>;

    Task SaveAsync();
}