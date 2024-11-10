using CodeFlix.Catalog.Infra.Data.EF;
using Microsoft.EntityFrameworkCore;
using DomainEntity = CodeFlix.Catalog.Domain.Entity;

namespace CodeFlix.Catalog.EndToEndTests.Api.Category.Common;

public class CategoryPersistence
{
    private readonly CodeFlixCatalogDbContext _dbContext;

    public CategoryPersistence(CodeFlixCatalogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<DomainEntity.Category?> GetByIdAsync(Guid id)
        => await _dbContext.Categories.AsNoTracking().FirstOrDefaultAsync(cat => cat.Id == id);
}
