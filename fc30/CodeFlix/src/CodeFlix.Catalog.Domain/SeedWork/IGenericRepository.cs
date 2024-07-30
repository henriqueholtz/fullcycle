namespace CodeFlix.Catalog.Domain.SeedWork;

public interface IGenericRepository<T> : IRepository where T : class
{
    public Task<T> InsertAsync(T aggregate, CancellationToken cancellationToken);
}
