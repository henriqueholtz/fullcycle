namespace CodeFlix.Catalog.UnitTests.Application.Category.UpdateCategory;

[Collection(nameof(UpdateCategoryTestsFixture))]
public class UpdateCategoryInputValidatorTests
{
    private readonly UpdateCategoryTestsFixture _fixture;

    public UpdateCategoryInputValidatorTests(UpdateCategoryTestsFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(DoNotValidateWhenEmptyGuid))]
    [Trait("Application", "UpdateCategoryInputValidatorTests - Use Cases")]
    public void DoNotValidateWhenEmptyGuid()
    {
        // Arrange
        var input = _fixture.GetValidInput(Guid.Empty);
        var validator = new UpdateCategoryInputValidator();

        // Act
        var result = validator.Validate(input);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors[0].ErrorMessage.Should().Be("'Id' must not be empty.");
    }

    [Fact(DisplayName = nameof(ValidateSuccessful))]
    [Trait("Application", "UpdateCategoryInputValidatorTests - Use Cases")]
    public void ValidateSuccessful()
    {
        // Arrange
        var input = _fixture.GetValidInput();
        var validator = new UpdateCategoryInputValidator();

        // Act
        var result = validator.Validate(input);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }
}
