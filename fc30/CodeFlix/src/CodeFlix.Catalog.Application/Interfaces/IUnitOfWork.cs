namespace CodeFlix.Catalog.Application;

public interface IUnitOfWork
{
    public Task CommitAsync(CancellationToken cancellationToken);
    public Task RollbackAsync(CancellationToken cancellationToken);
}
