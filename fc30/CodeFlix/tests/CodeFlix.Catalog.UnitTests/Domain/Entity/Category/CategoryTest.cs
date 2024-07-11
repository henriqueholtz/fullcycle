using CodeFlix.Catalog.Domain.Exceptions;
using DomainEntity = CodeFlix.Catalog.Domain.Entity;

namespace CodeFlix.Catalog.UnitTests.Domain.Entity.Category;

[Collection(nameof(CategoryTestFixture))]
public class CategoryTest(CategoryTestFixture _categoryFixture)
{

    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Instantiate()
    {
        // Arrange
        DomainEntity.Category validCategory = _categoryFixture.GetValidCategory();
        DateTime dateTimeBefore = DateTime.Now;

        // Act
        DomainEntity.Category category = new DomainEntity.Category(validCategory.Name, validCategory.Description);
        DateTime dateTimeAfter = DateTime.Now;

        // Assert
        category.Should().NotBeNull();
        category.Name.Should().Be(validCategory.Name);
        category.Description.Should().Be(validCategory.Description);
        category.Id.Should().NotBeEmpty();
        category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        category.CreatedAt.Should().BeAfter(dateTimeBefore);
        category.CreatedAt.Should().BeBefore(dateTimeAfter);
        category.IsActive.Should().BeTrue();
    }

    [Theory(DisplayName = nameof(InstantiateWithSpecificStatus))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void InstantiateWithSpecificStatus(bool isActive)
    {
        // Arrange
        DomainEntity.Category validCategory = _categoryFixture.GetValidCategory();
        DateTime dateTimeBefore = DateTime.Now.AddSeconds(-11);

        // Act
        DomainEntity.Category category = new DomainEntity.Category(validCategory.Name, validCategory.Description, isActive);
        DateTime dateTimeAfter = DateTime.Now.AddSeconds(1);

        // Assert
        category.Should().NotBeNull();
        category.Name.Should().Be(validCategory.Name);
        category.Description.Should().Be(validCategory.Description);
        category.Id.Should().NotBeEmpty();
        category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        category.CreatedAt.Should().BeAfter(dateTimeBefore);
        category.CreatedAt.Should().BeBefore(dateTimeAfter);
        category.IsActive.Should().Be(isActive);
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void InstantiateErrorWhenNameIsEmpty(string? name) 
    {
        // Arranje
        DomainEntity.Category validCategory = _categoryFixture.GetValidCategory();

        // Act
        Action action = () => new DomainEntity.Category(name!, validCategory.Description);

        // Assert
        action.Should().Throw<EntityValidationException>().WithMessage("Name should not be empty or null");
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsNull))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenDescriptionIsNull()
    {
        // Arranje
        DomainEntity.Category validCategory = _categoryFixture.GetValidCategory();

        // Act
        Action action = () => new DomainEntity.Category(validCategory.Name, null!);

        // Assert
        action.Should().Throw<EntityValidationException>().WithMessage("Description should not be null");
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsLessThan3Chars))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("a")]
    [InlineData("ab")]
    public void InstantiateErrorWhenNameIsLessThan3Chars(string name)
    {
        // Arranje
        DomainEntity.Category validCategory = _categoryFixture.GetValidCategory();

        // Act
        Action action = () => new DomainEntity.Category(name, validCategory.Description);

        // Assert
        action.Should().Throw<EntityValidationException>().WithMessage("Name should be at least 3 characters long");
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenNameIsGreaterThan255Chars))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenNameIsGreaterThan255Chars()
    {
        // Arranje
        DomainEntity.Category validCategory = _categoryFixture.GetValidCategory();

        // Act
        string invalidName = String.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());
        Action action = () => new DomainEntity.Category(invalidName, validCategory.Description);

        // Assert
        action.Should().Throw<EntityValidationException>().WithMessage("Name should be less or equal 255 characters long");
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsGreaterThan10_000Chars))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenDescriptionIsGreaterThan10_000Chars()
    {
        // Arranje
        DomainEntity.Category validCategory = _categoryFixture.GetValidCategory();

        // Act
        string invalidDescription = String.Join(null, Enumerable.Range(1, 10_001).Select(_ => "a").ToArray());
        Action action = () => new DomainEntity.Category(validCategory.Name, invalidDescription);

        // Assert
        action.Should().Throw<EntityValidationException>().WithMessage("Description should be less or equal 10.000 characters long");
    }

    [Fact(DisplayName = nameof(Activate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Activate()
    {
        // Arrange

        DomainEntity.Category validCategory = _categoryFixture.GetValidCategory();
        DomainEntity.Category category = new DomainEntity.Category(validCategory.Name, validCategory.Description, false);

        // Act
        category.Activate();

        // Assert
        category.IsActive.Should().BeTrue();
    }

    [Fact(DisplayName = nameof(Deactivate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Deactivate()
    {
        // Arrange
        DomainEntity.Category validCategory = _categoryFixture.GetValidCategory();
        DomainEntity.Category category = new DomainEntity.Category(validCategory.Name, validCategory.Description, true);

        // Act
        category.Deactivate();

        // Assert
        category.IsActive.Should().BeFalse();
    }

    [Fact(DisplayName = nameof(UpdateNameAndDescription))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateNameAndDescription()
    {
        // Arrange
        DomainEntity.Category validCategory = _categoryFixture.GetValidCategory();
        var newValues = new { Name = "Bar Category", Description = "Updated desc" };

        // Act
        validCategory.Update(newValues.Name, newValues.Description);

        // Assert
        validCategory.Name.Should().Be(newValues.Name);
        validCategory.Description.Should().Be(newValues.Description);
    }

    [Fact(DisplayName = nameof(UpdateOnlyName))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateOnlyName()
    {
        // Arrange
        DomainEntity.Category validCategory = _categoryFixture.GetValidCategory();
        string currentDescription = validCategory.Description;
        var newValues = new { Name = "Bar Category" };

        // Act
        validCategory.Update(newValues.Name);

        // Assert
        validCategory.Name.Should().Be(newValues.Name);
        validCategory.Description.Should().Be(currentDescription);
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void UpdateErrorWhenNameIsEmpty(string? name)
    {
        // Arranje
        DomainEntity.Category validCategory = _categoryFixture.GetValidCategory();

        // Act
        Action action = () => validCategory.Update(name!);

        // Assert
        action.Should().Throw<EntityValidationException>().WithMessage("Name should not be empty or null");
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsLessThan3Chars))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("a")]
    [InlineData("ab")]
    public void UpdateErrorWhenNameIsLessThan3Chars(string name)
    {
        // Arranje
        DomainEntity.Category validCategory = _categoryFixture.GetValidCategory();

        // Act
        Action action = () => validCategory.Update(name!);

        // Assert
        action.Should().Throw<EntityValidationException>().WithMessage("Name should be at least 3 characters long");
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenNameIsGreaterThan255Chars))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhenNameIsGreaterThan255Chars()
    {
        // Arranje
        DomainEntity.Category validCategory = _categoryFixture.GetValidCategory();
        string invalidName = String.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());
        
        // Act
        Action action = () => validCategory.Update(invalidName);

        // Assert
        action.Should().Throw<EntityValidationException>().WithMessage("Name should be less or equal 255 characters long");
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenDescriptionIsGreaterThan10_000Chars))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhenDescriptionIsGreaterThan10_000Chars()
    {
        // Arranje
        DomainEntity.Category validCategory = _categoryFixture.GetValidCategory();
        string invalidDescription = String.Join(null, Enumerable.Range(1, 10_001).Select(_ => "a").ToArray());
        
        // Act
        Action action = () => validCategory.Update(validCategory.Description, invalidDescription);

        // Assert
        action.Should().Throw<EntityValidationException>().WithMessage("Description should be less or equal 10.000 characters long");
    }
}
