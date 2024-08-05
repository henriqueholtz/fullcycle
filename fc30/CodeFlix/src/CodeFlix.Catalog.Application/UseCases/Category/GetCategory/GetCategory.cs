using CodeFlix.Catalog.Domain.Repository;
using MediatR;
using DomainEntity = CodeFlix.Catalog.Domain.Entity;

namespace CodeFlix.Catalog.Application.UseCases.Category.GetCategory;

public class GetCategory : IRequestHandler<GetCategoryInput, GetCategoryOutput>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategory(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<GetCategoryOutput> Handle(GetCategoryInput request, CancellationToken cancellationToken)
    {
        DomainEntity.Category category = await _categoryRepository.GetAsync(request.Id, cancellationToken);
        return GetCategoryOutput.FromCategory(category);
    }
}
