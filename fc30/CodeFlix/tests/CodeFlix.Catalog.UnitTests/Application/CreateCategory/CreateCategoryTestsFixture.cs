using CodeFlix.Catalog.Application;
using CodeFlix.Catalog.Application.UseCases.Category.CreateCategory;
using CodeFlix.Catalog.Domain.Repository;
using CodeFlix.Catalog.UnitTests.Common;

namespace CodeFlix.Catalog.UnitTests;

[CollectionDefinition(nameof(CreateCategoryTestsFixture))]
public class CreateCategoryTestsFixtureCollection : ICollectionFixture<CreateCategoryTestsFixture> 
{ }

public class CreateCategoryTestsFixture : BaseFixture
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

    public bool GetRandomBoolean() => (new Random()).NextDouble() > 0.5;

    public CreateCategoryInput GetValidInput() => new CreateCategoryInput(
        GetValidCategoryName(),
        GetValidCategoryDescription(),
        GetRandomBoolean()
    );

    public CreateCategoryInput GetInvalidInputShortName() 
    {
        CreateCategoryInput invalidInput = GetValidInput();
        invalidInput.Name = invalidInput.Name.Substring(0, 2);
        return invalidInput;
    }

    public CreateCategoryInput GetInvalidInputTooLongName() 
    {
        CreateCategoryInput invalidInput = GetValidInput();
        while (invalidInput.Name.Length <= 255)
            invalidInput.Name += $" {GetValidCategoryName()}";
        return invalidInput;
    }

    public CreateCategoryInput GetInvalidInputNullDescription() 
    {
        CreateCategoryInput invalidInput = GetValidInput();
        invalidInput.Description = null!;
        return invalidInput;
    }

    public CreateCategoryInput GetInvalidInputTooLongDescription() 
    {
        CreateCategoryInput invalidInput = GetValidInput();
        invalidInput = GetValidInput();
        while (invalidInput.Description.Length <= 10_000)
            invalidInput.Description += $" {GetValidCategoryDescription()}";
        return invalidInput;
    }
 }
