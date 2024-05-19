using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SonseArt.Areas.Identity.Data;
using SonseArt.Models;

namespace SonseArt.Data;

public class SonseArtContext : IdentityDbContext<User>
{
    public SonseArtContext(DbContextOptions<SonseArtContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }

public DbSet<SonseArt.Models.Product> Product { get; set; } = default!;
public DbSet<SonseArt.Models.Comment> Comment { get; set; } = default!;
public DbSet<SonseArt.Models.Cart> Cart { get; set; } = default!;
public DbSet<SonseArt.Models.CartItem> CartItem { get; set; } = default!;
public DbSet<SonseArt.Models.Order> Order { get; set; } = default!;
public DbSet<SonseArt.Models.OrderItem> OrderItem {  get; set; } = default!;
}
