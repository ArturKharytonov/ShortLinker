using UrlShortener.DAL.Entities.Common;

namespace UrlShortener.DAL.Entities;

public class Url : IDbEntity<string>
{
    public string Id { get; set; }
    public string LongUrl { get; set; } = null!;
    public string ShortUrl { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
}
