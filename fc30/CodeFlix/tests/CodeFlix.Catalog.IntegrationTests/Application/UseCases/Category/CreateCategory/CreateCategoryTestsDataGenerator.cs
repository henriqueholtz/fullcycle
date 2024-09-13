using CodeFlix.Catalog.Application.UseCases.Category.CreateCategory;

namespace CodeFlix.Catalog.IntegrationTests.Application.UseCases.Category.CreateCategory;

public class CreateCategoryTestsDataGenerator
{
    public static IEnumerable<object[]> GetValidInputs()
    {
        CreateCategoryTestsFixture fixture = new();
        return new List<object[]>() {
            new [] { fixture.GetValidInput() },
            new [] { new CreateCategoryInput(fixture.GetValidCategoryName()) },
            new [] { new CreateCategoryInput(fixture.GetValidCategoryName(), fixture.GetValidCategoryDescription()) }
        };
    }

    public static IEnumerable<object[]> GetInvalidInputs(int times = 12)
    {
        var fixture = new CreateCategoryTestsFixture();
        List<object[]> invalidInputs = new();
        int totalInvalidInputs = 4;

        for (int index = 0; index < times; index++)
        {
            CreateCategoryInput? invalidInput;
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
                case 2: // Null descrition
                    invalidInput = fixture.GetInvalidInputNullDescription();
                    invalidInputs.Add(new object[] { invalidInput, "Description should not be null" });
                    break;
                case 3: // Description too long
                    invalidInput = fixture.GetInvalidInputTooLongDescription();
                    invalidInputs.Add(new object[] { invalidInput, "Description should be less or equal 10000 characters long" });
                    break;
            }
        }

        return invalidInputs;
    }
}
