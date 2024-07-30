namespace CodeFlix.Catalog.Application;

public interface IUnitOfWork
{
    public Task CommitAsync(CancellationToken cancellationToken);
}
