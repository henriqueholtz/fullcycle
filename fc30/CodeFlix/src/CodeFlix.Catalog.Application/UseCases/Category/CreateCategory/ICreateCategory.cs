namespace CodeFlix.Catalog.Application.UseCases.Category.CreateCategory;

public interface ICreateCategory
{
    public Task<CreateCategoryOutput> HandleAsync(CreateCategoryInput input, CancellationToken cancellationToken);
}
