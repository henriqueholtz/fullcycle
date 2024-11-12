using CodeFlix.Catalog.Application.UseCases.Category.CreateCategory;

namespace CodeFlix.Catalog.EndToEndTests.Api.Category.CreateCategory;

public class CreateCategoryApiTestsDataGenerator
{
    public static IEnumerable<object[]> GetValidInputs()
    {
        CreateCategoryApiTestsFixture fixture = new();
        return new List<object[]>() {
            new [] { fixture.GetValidInput() },
            new [] { new CreateCategoryInput(fixture.GetValidCategoryName()) },
            new [] { new CreateCategoryInput(fixture.GetValidCategoryName(), fixture.GetValidCategoryDescription()) }
        };
    }

    public static IEnumerable<object[]> GetInvalidInputs()
    {
        int times = 3;
        var fixture = new CreateCategoryApiTestsFixture();
        List<object[]> invalidInputs = new();
        int totalInvalidInputs = 4;

        for (int index = 0; index < times; index++)
        {
            CreateCategoryInput? inputToBeInvalid;
            switch (index % totalInvalidInputs)
            {
                case 0: // Name too short
                    inputToBeInvalid = fixture.GetValidInput();
                    inputToBeInvalid.Name = fixture.GetInvalidNameTooShort();
                    invalidInputs.Add(new object[] { inputToBeInvalid, "Name should be at least 3 characters long" });
                    break;
                case 1: // Name too long
                    inputToBeInvalid = fixture.GetValidInput();
                    inputToBeInvalid.Name = fixture.GetInvalidNameTooLong();
                    invalidInputs.Add(new object[] { inputToBeInvalid, "Name should be less or equal 255 characters long" });
                    break;
                case 2: // Description too long
                    inputToBeInvalid = fixture.GetValidInput();
                    inputToBeInvalid.Description = fixture.GetInvalidDescriptionTooLong();
                    invalidInputs.Add(new object[] { inputToBeInvalid, "Description should be less or equal 10000 characters long" });
                    break;
            }
        }

        return invalidInputs;
    }
}
