namespace CodeFlix.Catalog.Domain.SeedWork;

public interface IGenericRepository<TAggregate> : IRepository where TAggregate : AggregateRoot
{
    public Task<TAggregate> GetAsync(Guid id, CancellationToken cancellationToken);
    public Task InsertAsync(TAggregate aggregate, CancellationToken cancellationToken);
    public Task UpdateAsync(TAggregate aggregate, CancellationToken cancellationToken);
    public Task DeleteAsync(TAggregate aggregate, CancellationToken cancellationToken);
}
