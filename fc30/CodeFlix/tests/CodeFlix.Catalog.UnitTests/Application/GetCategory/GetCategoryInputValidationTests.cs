using CodeFlix.Catalog.Application.UseCases.Category.GetCategory;

namespace CodeFlix.Catalog.UnitTests.Application.GetCategory;

[Collection(nameof(GetCategoryTestsFixture))]
public class GetCategoryInputValidationTests
{
    private readonly GetCategoryTestsFixture _fixture;

    public GetCategoryInputValidationTests(GetCategoryTestsFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(Success))]
    [Trait("Application", "GetCategoryInputValidation - Use Cases")]
    public void Success() {
        // Arranje
        var validInput = new GetCategoryInput(Guid.NewGuid());
        var validator = new GetCategoryInputValidator();

        // Act
        var validationResult = validator.Validate(validInput);

        // Assert
        validationResult.Should().NotBeNull();
        validationResult.IsValid.Should().BeTrue();
        validationResult.Errors.Should().BeEmpty();
    }

    [Fact(DisplayName = nameof(InvalidWhenEmptyGuidId))]
    [Trait("Application", "GetCategoryInputValidation - Use Cases")]
    public void InvalidWhenEmptyGuidId() {
        // Arranje
        var invalidInput = new GetCategoryInput(Guid.Empty);
        var validator = new GetCategoryInputValidator();

        // Act
        var validationResult = validator.Validate(invalidInput);

        // Assert
        validationResult.Should().NotBeNull();
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().HaveCount(1);
        validationResult.Errors[0].ErrorMessage.Should().Be("'Id' must not be empty.");
    }
}
