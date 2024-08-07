﻿using DomainEntity = CodeFlix.Catalog.Domain.Entity;
using CodeFlix.Catalog.Domain.Repository;
using CodeFlix.Catalog.Application.UseCases.Category.Common;

namespace CodeFlix.Catalog.Application.UseCases.Category.CreateCategory;

public class CreateCategory : ICreateCategory
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCategory(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CategoryModelOutput> Handle(CreateCategoryInput input, CancellationToken cancellationToken)
    {
        var category = new DomainEntity.Category(input.Name, input.Description, input.IsActive);

        await _categoryRepository.InsertAsync(category, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return CategoryModelOutput.FromCategory(category);
    }
}
