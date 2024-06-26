﻿using CodeFlix.Catalog.Domain.Exceptions;
using DomainEntity = CodeFlix.Catalog.Domain.Entity;

namespace CodeFlix.Catalog.UnitTests.Domain.Entity.Category;

public class CategoryTest
{
    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Instantiate()
    {
        // Arrange
        var validData = new
        {
            Name = "Foo Category",
            Description = "Description"
        };
        DateTime dateTimeBefore = DateTime.Now;

        // Act
        DomainEntity.Category category = new DomainEntity.Category(validData.Name, validData.Description);
        DateTime dateTimeAfter = DateTime.Now;

        // Assert
        Assert.NotNull(category);
        Assert.Equal(validData.Name, category.Name);
        Assert.Equal(validData.Description, category.Description);
        Assert.NotEqual(Guid.Empty, category.Id);
        Assert.NotEqual(default(DateTime), category.CreatedAt);
        Assert.True(category.CreatedAt > dateTimeBefore);
        Assert.True(category.CreatedAt < dateTimeAfter);
        Assert.True(category.IsActive);
    }

    [Theory(DisplayName = nameof(InstantiateWithSpecificStatus))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void InstantiateWithSpecificStatus(bool isActive)
    {
        // Arrange
        var validData = new
        {
            Name = "Foo Category",
            Description = "Description"
        };
        DateTime dateTimeBefore = DateTime.Now.AddSeconds(-11);

        // Act
        DomainEntity.Category category = new DomainEntity.Category(validData.Name, validData.Description, isActive);
        DateTime dateTimeAfter = DateTime.Now.AddSeconds(1);

        // Assert
        Assert.NotNull(category);
        Assert.Equal(validData.Name, category.Name);
        Assert.Equal(validData.Description, category.Description);
        Assert.NotEqual(Guid.Empty, category.Id);
        Assert.NotEqual(default(DateTime), category.CreatedAt);
        Assert.True(category.CreatedAt > dateTimeBefore);
        Assert.True(category.CreatedAt < dateTimeAfter);
        Assert.Equal(isActive, category.IsActive);
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void InstantiateErrorWhenNameIsEmpty(string? name) 
    {
        Action action = () => new DomainEntity.Category(name!, "Valid description");

        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should not be empty or null", exception.Message);
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsNull))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenDescriptionIsNull()
    {
        Action action = () => new DomainEntity.Category("Sports", null!);

        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Description should not be null", exception.Message);
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsLessThan3Chars))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("a")]
    [InlineData("ab")]
    public void InstantiateErrorWhenNameIsLessThan3Chars(string name)
    {
        Action action = () => new DomainEntity.Category(name, "Valid description");

        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should be at least 3 characters long", exception.Message);
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenNameIsGreaterThan255Chars))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenNameIsGreaterThan255Chars()
    {
        string invalidName = String.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());
        Action action = () => new DomainEntity.Category(invalidName, "Valid description");

        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should be less or equal 255 characters long", exception.Message);
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsGreaterThan10_000Chars))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenDescriptionIsGreaterThan10_000Chars()
    {
        string invalidDescription = String.Join(null, Enumerable.Range(1, 10_001).Select(_ => "a").ToArray());
        Action action = () => new DomainEntity.Category("Sports", invalidDescription);

        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Description should be less or equal 10.000 characters long", exception.Message);
    }

    [Fact(DisplayName = nameof(Activate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Activate()
    {
        // Arrange
        var validData = new
        {
            Name = "Foo Category",
            Description = "Description"
        };
        DomainEntity.Category category = new DomainEntity.Category(validData.Name, validData.Description, false);

        // Act
        category.Activate();

        // Assert
        Assert.True(category.IsActive);
    }

    [Fact(DisplayName = nameof(Deactivate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Deactivate()
    {
        // Arrange
        var validData = new
        {
            Name = "Foo Category",
            Description = "Description"
        };
        DomainEntity.Category category = new DomainEntity.Category(validData.Name, validData.Description, true);

        // Act
        category.Deactivate();

        // Assert
        Assert.False(category.IsActive);
    }

    [Fact(DisplayName = nameof(UpdateNameAndDescription))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateNameAndDescription()
    {
        // Arrange
        DomainEntity.Category category = new DomainEntity.Category("Foo Category", "Description");
        var newValues = new { Name = "Bar Category", Description = "Updated desc" };

        // Act
        category.Update(newValues.Name, newValues.Description);

        // Assert
        Assert.Equal(newValues.Name, category.Name);
        Assert.Equal(newValues.Description, category.Description);
    }

    [Fact(DisplayName = nameof(UpdateName))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateName()
    {
        // Arrange
        string currentDescription = "Description";
        DomainEntity.Category category = new DomainEntity.Category("Foo Category", currentDescription);
        var newValues = new { Name = "Bar Category" };

        // Act
        category.Update(newValues.Name);

        // Assert
        Assert.Equal(newValues.Name, category.Name);
        Assert.Equal(currentDescription, category.Description);
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void UpdateErrorWhenNameIsEmpty(string? name)
    {
        // Arranje
        DomainEntity.Category category = new DomainEntity.Category("Foo Category", "Description");
        
        // Act
        Action action = () => category.Update(name!);

        // Assert
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should not be empty or null", exception.Message);
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsLessThan3Chars))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("a")]
    [InlineData("ab")]
    public void UpdateErrorWhenNameIsLessThan3Chars(string name)
    {
        // Arranje
        DomainEntity.Category category = new DomainEntity.Category("Foo Category", "Description");

        // Act
        Action action = () => category.Update(name!);

        // Assert
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should be at least 3 characters long", exception.Message);
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenNameIsGreaterThan255Chars))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhenNameIsGreaterThan255Chars()
    {
        // Arranje
        DomainEntity.Category category = new DomainEntity.Category("Foo Category", "Description");
        string invalidName = String.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());
        
        // Act
        Action action = () => category.Update(invalidName);

        // Assert
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should be less or equal 255 characters long", exception.Message);
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenDescriptionIsGreaterThan10_000Chars))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhenDescriptionIsGreaterThan10_000Chars()
    {
        // Arranje
        string categoryName = "Foo Category";
        DomainEntity.Category category = new DomainEntity.Category(categoryName, "Description");
        string invalidDescription = String.Join(null, Enumerable.Range(1, 10_001).Select(_ => "a").ToArray());
        
        // Act
        Action action = () => category.Update(categoryName, invalidDescription);

        // Assert
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Description should be less or equal 10.000 characters long", exception.Message);
    }
}
