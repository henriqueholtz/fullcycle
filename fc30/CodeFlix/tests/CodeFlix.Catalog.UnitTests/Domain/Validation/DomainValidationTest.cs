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

    [Theory(DisplayName = nameof(MinLengthThrowWhenLess))]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetValuesSmallerThanMinimum), parameters: 10)]
    public void MinLengthThrowWhenLess(string? target, int minLength)
    {
        // Arranje
        string? value = target;

        // Act
        Action action = () => DomainValidation.MinLength(value, minLength, "FieldName");

        // Assert
        action.Should().Throw<EntityValidationException>()
            .WithMessage($"FieldName should not be less than {minLength} characters long");
    }

    public static IEnumerable<object[]> GetValuesSmallerThanMinimum(int numberOfTests = 5)
    {
        var faker = new Faker();
        for (int i = 0; i < numberOfTests; i++)
        {
            var example = faker.Commerce.ProductName();
            var minLength = example.Length + (new Random()).Next(1, 20);
            yield return new object[] { example, minLength };
        }
    }

    [Theory(DisplayName = nameof(MinLengthOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetValuesGreaterThanMinimum), parameters: 6)]
    public void MinLengthOk(string? target, int minLength)
    {
        // Act
        Action action = () => DomainValidation.MinLength(target, minLength, "Value");

        // Assert
        action.Should().NotThrow();
    }

    public static IEnumerable<object[]> GetValuesGreaterThanMinimum(int numberOfTests = 5)
    {
        var faker = new Faker();
        for (int i = 0; i < numberOfTests; i++)
        {
            var example = faker.Commerce.ProductName();
            var minLength = example.Length - (new Random()).Next(1, 5);
            if (minLength < 0)
                minLength = 0;

            yield return new object[] { example, minLength };
        }
    }
}
