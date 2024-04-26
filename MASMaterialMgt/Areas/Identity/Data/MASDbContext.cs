using MASMaterialMgt.Areas.Identity.Data;
using MASMaterialMgt.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MASMaterialMgt.Data;

public class MASDbContext : IdentityDbContext<ApplicationUser>
{
    public MASDbContext(DbContextOptions<MASDbContext> options)
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

    public DbSet<Material> Materials { get; set; }

    public DbSet<Supplier> Suppliers { get; set; }
}
