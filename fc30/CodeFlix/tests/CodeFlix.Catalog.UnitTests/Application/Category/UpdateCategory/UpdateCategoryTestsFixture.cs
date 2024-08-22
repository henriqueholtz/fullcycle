using CodeFlix.Catalog.Application.UseCases.Category.UpdateCategory;
using CodeFlix.Catalog.UnitTests.Application.Category.Common;

namespace CodeFlix.Catalog.UnitTests.Application.Category.UpdateCategory;

[CollectionDefinition(nameof(UpdateCategoryTestsFixture))]
public class UpdateCategoryTestsFixtureCollection : ICollectionFixture<UpdateCategoryTestsFixture> { }

public class UpdateCategoryTestsFixture : CategoryUseCasesBaseFixture
{

    public UpdateCategoryInput GetValidInput(Guid? categoryId = null)
        => new(
            categoryId ?? Guid.NewGuid(),
            GetValidCategoryName(),
            GetValidCategoryDescription(),
            GetRandomBoolean()
        );

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
