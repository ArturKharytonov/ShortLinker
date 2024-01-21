using Microsoft.AspNetCore.Identity;
using UrlShortener.DAL.Entities.Common;

namespace UrlShortener.DAL.Entities;

public class User : IdentityUser<int>, IDbEntity<int>
{
    public ICollection<Url> Urls { get; set; } = new List<Url>();
}