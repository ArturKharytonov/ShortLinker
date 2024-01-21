namespace UrlShortener.DAL.Common.Interfaces;

public interface IRepository<TEntity, TId>
{
    Task<TEntity?> GetByIdAsync(TId id);
    Task CreateAsync(TEntity entity);
    Task DeleteAsync(TId id);
    IQueryable<TEntity> GetAllAsQueryable();
}