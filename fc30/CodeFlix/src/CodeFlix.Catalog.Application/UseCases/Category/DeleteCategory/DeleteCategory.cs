using DomainEntity = CodeFlix.Catalog.Domain.Entity;
using CodeFlix.Catalog.Domain.Repository;

namespace CodeFlix.Catalog.Application.UseCases.Category.DeleteCategory;

public class DeleteCategory : IDeleteCategory
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCategory(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteCategoryInput request, CancellationToken cancellationToken)
    {
        DomainEntity.Category category = await _categoryRepository.GetAsync(request.Id, cancellationToken);
        await _categoryRepository.DeleteAsync(category, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}
