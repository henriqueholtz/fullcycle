using CodeFlix.Catalog.Domain.Entity;
using CodeFlix.Catalog.Infra.Data.EF.Configurations;
using Microsoft.EntityFrameworkCore;

namespace CodeFlix.Catalog.Infra.Data.EF;

public class CodeFlixCatalogDbContext : DbContext
{
    public DbSet<Category> Categories => Set<Category>();
    public CodeFlixCatalogDbContext(DbContextOptions<CodeFlixCatalogDbContext> options) : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CategoryConfiguration).Assembly);
    }
}
