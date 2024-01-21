namespace UrlShortener.DAL.Entities.Common;

public interface IDbEntity<TId>
{
    public TId Id { get; set; }
}