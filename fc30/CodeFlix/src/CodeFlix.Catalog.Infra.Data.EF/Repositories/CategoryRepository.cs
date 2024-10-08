using CodeFlix.Catalog.Application.Exceptions;
using CodeFlix.Catalog.Domain.Entity;
using CodeFlix.Catalog.Domain.Repository;
using CodeFlix.Catalog.Domain.SeedWork.SearchableRepository;
using Microsoft.EntityFrameworkCore;

namespace CodeFlix.Catalog.Infra.Data.EF.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly CodeFlixCatalogDbContext _dbContext;
    private DbSet<Category> _categories => _dbContext.Set<Category>();

    public CategoryRepository(CodeFlixCatalogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task DeleteAsync(Category aggregate, CancellationToken _)
    {
        return Task.FromResult(_categories.Remove(aggregate));
    }

    public async Task<Category> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var category = await _categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        NotFoundException.ThrowIfNull(category, $"Category {id} not found.");

        return category!;
    }

    public async Task InsertAsync(Category aggregate, CancellationToken cancellationToken)
    {
        await _categories.AddAsync(aggregate);
    }

    private IQueryable<Category> AddOrderToQuery(IQueryable<Category> query, string orderBy, SearchOrder order)
    {
        return (orderBy.ToLower(), order) switch
        {
            ("name", SearchOrder.Asc) => query.OrderBy(c => c.Name),
            ("name", SearchOrder.Desc) => query.OrderByDescending(c => c.Name),
            ("id", SearchOrder.Asc) => query.OrderBy(c => c.Id),
            ("id", SearchOrder.Desc) => query.OrderByDescending(c => c.Id),
            ("createdat", SearchOrder.Asc) => query.OrderBy(c => c.CreatedAt),
            ("createdat", SearchOrder.Desc) => query.OrderByDescending(c => c.CreatedAt),
            _ => query.OrderBy(c => c.Name),
        };
    }

    public async Task<SearchOutput<Category>> SearchAsync(SearchInput searchInput, CancellationToken cancellationToken)
    {
        var query = _categories.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(searchInput.Search))
            query = query.Where(c => c.Name.Contains(searchInput.Search));

        query = AddOrderToQuery(query, searchInput.OrderBy, searchInput.Order);

        var skipQuantity = (searchInput.Page - 1) * searchInput.PerPage;
        var total = await query.CountAsync();
        var categories = await query.Skip(skipQuantity).Take(searchInput.PerPage).ToListAsync();
        return new SearchOutput<Category>(searchInput.Page, searchInput.PerPage, total, categories);
    }

    public Task UpdateAsync(Category aggregate, CancellationToken _)
    {
        return Task.FromResult(_categories.Update(aggregate));
    }
}
