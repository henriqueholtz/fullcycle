using CodeFlix.Catalog.Application.UseCases.Category.CreateCategory;
using CodeFlix.Catalog.UnitTests.Application.Common;

namespace CodeFlix.Catalog.UnitTests;

[CollectionDefinition(nameof(CreateCategoryTestsFixture))]
public class CreateCategoryTestsFixtureCollection : ICollectionFixture<CreateCategoryTestsFixture> 
{ }

public class CreateCategoryTestsFixture : CategoryUseCasesBaseFixture
{
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
