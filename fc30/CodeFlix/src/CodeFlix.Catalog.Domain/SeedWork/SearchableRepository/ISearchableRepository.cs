namespace CodeFlix.Catalog.Domain.SeedWork.SearchableRepository;

public interface ISearchableRepository<TAggregate> where TAggregate : AggregateRoot
{
    public Task<SearchOutput<TAggregate>> SearchAsync(SearchInput searchInput, CancellationToken cancellationToken);
}
