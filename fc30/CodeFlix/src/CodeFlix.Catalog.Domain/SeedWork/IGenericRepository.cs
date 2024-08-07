namespace CodeFlix.Catalog.Domain.SeedWork;

public interface IGenericRepository<TAggregate> : IRepository where TAggregate : class
{
    public Task<TAggregate> InsertAsync(TAggregate aggregate, CancellationToken cancellationToken);
    public Task<TAggregate> GetAsync(Guid id, CancellationToken cancellationToken);
    public Task<TAggregate> DeleteAsync(TAggregate aggregate, CancellationToken cancellationToken);
}
