using CodeFlix.Catalog.Application;

namespace CodeFlix.Catalog.Infra.Data.EF;

public class UnitOfWork : IUnitOfWork
{
    private readonly CodeFlixCatalogDbContext _dbContext;

    public UnitOfWork(CodeFlixCatalogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task RollbackAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
