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
        string fieldName = this._faker.Commerce.ProductName().Replace(" ", "");

        // Act
        Action action = () => DomainValidation.NotNull(value, fieldName);

        // Assert
        action.Should().Throw<EntityValidationException>()
            .WithMessage($"{fieldName} should not be null");
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
        string fieldName = this._faker.Commerce.ProductName().Replace(" ", "");

        // Act
        Action action = () => DomainValidation.NotNullOrEmpty(value, fieldName);

        // Assert
        action.Should().Throw<EntityValidationException>()
            .WithMessage($"{fieldName} should not be empty or null");
    }

    [Fact(DisplayName = nameof(NotNullOrEmptyOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNullOrEmptyOk()
    {
        // Arranje
        var value = _faker.Commerce.ProductName();
        string fieldName = this._faker.Commerce.ProductName().Replace(" ", "");

        // Act
        Action action = () => DomainValidation.NotNullOrEmpty(value, fieldName);

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
        string fieldName = this._faker.Commerce.ProductName().Replace(" ", "");

        // Act
        Action action = () => DomainValidation.MinLength(value, minLength, fieldName);

        // Assert
        action.Should().Throw<EntityValidationException>()
            .WithMessage($"{fieldName} should be at least {minLength} characters long");
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
        // Arranje 
        string fieldName = this._faker.Commerce.ProductName().Replace(" ", "");

        // Act
        Action action = () => DomainValidation.MinLength(target, minLength, fieldName);

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

    [Theory(DisplayName = nameof(MaxLengthThrowWhenGreater))]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetValuesGreaterThanMaximum), parameters: 10)]
    public void MaxLengthThrowWhenGreater(string? target, int maxLength)
    {
        // Arranje
        string? value = target;
        string fieldName = this._faker.Commerce.ProductName().Replace(" ", "");

        // Act
        Action action = () => DomainValidation.MaxLength(value, maxLength, fieldName);

        // Assert
        action.Should().Throw<EntityValidationException>()
            .WithMessage($"{fieldName} should be less or equal {maxLength} characters long");
    }

    public static IEnumerable<object[]> GetValuesGreaterThanMaximum(int numberOfTests = 5)
    {
        var faker = new Faker();
        for (int i = 0; i < numberOfTests; i++)
        {
            var example = faker.Commerce.ProductName();
            var maxLength = example.Length - (new Random()).Next(1, 5);
            if (maxLength < 0)
                maxLength = 0;

            yield return new object[] { example, maxLength };
        }
    }

    [Theory(DisplayName = nameof(MaxLengthOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetValuesLessThanMaximum), parameters: 10)]
    public void MaxLengthOk(string? target, int maxLength)
    {
        // Arranje
        string? value = target;
        string fieldName = this._faker.Commerce.ProductName().Replace(" ", "");

        // Act
        Action action = () => DomainValidation.MaxLength(value, maxLength, fieldName);

        // Assert
        action.Should().NotThrow<EntityValidationException>();
    }

    public static IEnumerable<object[]> GetValuesLessThanMaximum(int numberOfTests = 5)
    {
        var faker = new Faker();
        for (int i = 0; i < numberOfTests; i++)
        {
            var example = faker.Commerce.ProductName();
            var maxLength = example.Length + (new Random()).Next(0, 5);
            if (maxLength < 0)
                maxLength = 0;

            yield return new object[] { example, maxLength };
        }
    }
}
