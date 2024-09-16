using CodeFlix.Catalog.Application.UseCases.Category.UpdateCategory;

namespace CodeFlix.Catalog.IntegrationTests.Application.UseCases.Category.UpdateCategory;

public class UpdateCategoryTestsGenerator
{
    public static IEnumerable<object[]> GetCategoriesToUpdate(int quantity)
    {
        var fixture = new UpdateCategoryTestsFixture();
        for (int i = 0; i < quantity; i++)
        {
            var category = fixture.GetValidCategory();
            var input = fixture.GetValidInput(category.Id);
            yield return new object[] { category, input };
        }
    }

    public static IEnumerable<object[]> GetInvalidInputs(int times)
    {
        var fixture = new UpdateCategoryTestsFixture();
        List<object[]> invalidInputs = new();
        int totalInvalidInputs = 3;

        for (int index = 0; index < times; index++)
        {
            UpdateCategoryInput? invalidInput;
            switch (index % totalInvalidInputs)
            {
                case 0: // Name too short
                    invalidInput = fixture.GetInvalidInputShortName();
                    invalidInputs.Add(new object[] { invalidInput, "Name should be at least 3 characters long" });
                    break;
                case 1: // Name too long
                    invalidInput = fixture.GetInvalidInputTooLongName();
                    invalidInputs.Add(new object[] { invalidInput, "Name should be less or equal 255 characters long" });
                    break;
                case 2: // Description too long
                    invalidInput = fixture.GetInvalidInputTooLongDescription();
                    invalidInputs.Add(new object[] { invalidInput, "Description should be less or equal 10000 characters long" });
                    break;
            }
        }

        return invalidInputs;
    }

}
