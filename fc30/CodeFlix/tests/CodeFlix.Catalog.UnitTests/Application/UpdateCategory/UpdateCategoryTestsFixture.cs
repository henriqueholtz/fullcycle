using CodeFlix.Catalog.Application;
using CodeFlix.Catalog.Application.UseCases.Category.UpdateCategory;
using CodeFlix.Catalog.Domain.Repository;
using CodeFlix.Catalog.UnitTests.Common;
using DomainEntity = CodeFlix.Catalog.Domain.Entity;

namespace CodeFlix.Catalog.UnitTests.Application.UpdateCategory;

[CollectionDefinition(nameof(UpdateCategoryTestsFixture))]
public class UpdateCategoryTestsFixtureCollection : ICollectionFixture<UpdateCategoryTestsFixture> {}

public class UpdateCategoryTestsFixture : BaseFixture
{    
    public Mock<ICategoryRepository> GetRepositoryMock() => new();
    public Mock<IUnitOfWork> GetUowMock() => new();

    public string GetValidCategoryName()
    {
        string categoryName = string.Empty;
        while (categoryName.Length < 2)
            categoryName = Faker.Commerce.Categories(1)[0];

        if (categoryName.Length > 255)
            categoryName = categoryName.Substring(0, 255);

        return categoryName;
    }

    public string GetValidCategoryDescription()
    {
        string categoryDescription = Faker.Commerce.ProductDescription();

        if (categoryDescription.Length > 10_000)
            categoryDescription = categoryDescription.Substring(0, 10_000);

        return categoryDescription;
    }

    public UpdateCategoryInput GetValidInput(Guid? categoryId = null)
        => new(
            categoryId ?? Guid.NewGuid(), 
            GetValidCategoryName(), 
            GetValidCategoryDescription(), 
            GetRandomBoolean()
        );

    public bool GetRandomBoolean() => (new Random()).NextDouble() > 0.5;

    public DomainEntity.Category GetValidCategory() => new(GetValidCategoryName(), GetValidCategoryDescription(), GetRandomBoolean());
    
    public UpdateCategoryInput GetInvalidInputShortName() 
    {
        UpdateCategoryInput invalidInput = GetValidInput();
        invalidInput.Name = invalidInput.Name.Substring(0, 2);
        return invalidInput;
    }

    public UpdateCategoryInput GetInvalidInputTooLongName() 
    {
        UpdateCategoryInput invalidInput = GetValidInput();
        while (invalidInput.Name.Length <= 255)
            invalidInput.Name += $" {GetValidCategoryName()}";
        return invalidInput;
    }

    public UpdateCategoryInput GetInvalidInputTooLongDescription() 
    {
        UpdateCategoryInput invalidInput = GetValidInput();
        invalidInput = GetValidInput();
        if (invalidInput.Description is null)
            invalidInput.Description = GetValidCategoryDescription();

        while (invalidInput.Description.Length <= 10_000)
            invalidInput.Description += $" {GetValidCategoryDescription()}";
        return invalidInput;
    }
}
