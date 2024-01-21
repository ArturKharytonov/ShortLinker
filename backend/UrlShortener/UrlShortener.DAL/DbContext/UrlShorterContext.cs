using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UrlShortener.DAL.Entities;

namespace UrlShortener.DAL.DbContext;

public class UrlShorterContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public UrlShorterContext()
    {
        
    }
    public UrlShorterContext(DbContextOptions<UrlShorterContext> options)
        : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Url> Urls { get; set; } = null!;
}