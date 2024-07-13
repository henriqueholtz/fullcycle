using Bogus;
using CodeFlix.Catalog.Domain.Exceptions;
using CodeFlix.Catalog.Domain.Validation;

namespace CodeFlix.Catalog.UnitTests.Domain.Validation;

public class DomainValidationTest
{
    private Faker _faker { get; set; } = new Faker();

    [Fact(DisplayName = nameof(NotNullOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNullOk()
    {
        // Arranje
        var value = _faker.Commerce.ProductName();

        // Act
        Action action = () => DomainValidation.NotNull(value, "Value");

        // Assert
        action.Should().NotThrow();
    }

    [Fact(DisplayName = nameof(NotNullThrowWhenNull))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNullThrowWhenNull()
    {
        // Arranje
        string? value = null;

        // Act
        Action action = () => DomainValidation.NotNull(value, "FieldName");

        // Assert
        action.Should().Throw<EntityValidationException>()
            .WithMessage("FieldName should not be null");
    }

    [Theory(DisplayName = nameof(NotEmptyOrNullThrowWhenEmpty))]
    [Trait("Domain", "DomainValidation - Validation")]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData(null)]
    public void NotEmptyOrNullThrowWhenEmpty(string? target)
    {
        // Arranje
        string? value = target;

        // Act
        Action action = () => DomainValidation.NotNullOrEmpty(value, "FieldName");

        // Assert
        action.Should().Throw<EntityValidationException>()
            .WithMessage("FieldName should not be null or empty");
    }

    [Fact(DisplayName = nameof(NotNullOrEmptyOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNullOrEmptyOk()
    {
        // Arranje
        var value = _faker.Commerce.ProductName();

        // Act
        Action action = () => DomainValidation.NotNullOrEmpty(value, "Value");

        // Assert
        action.Should().NotThrow();
    }
}
