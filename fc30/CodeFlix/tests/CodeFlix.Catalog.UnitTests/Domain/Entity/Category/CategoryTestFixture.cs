using CodeFlix.Catalog.UnitTests.Common;
using DomainEntity = CodeFlix.Catalog.Domain.Entity;

namespace CodeFlix.Catalog.UnitTests.Domain.Entity.Category;

public class CategoryTestFixture : BaseFixture
{
    public CategoryTestFixture() : base() { }

    public string GetValidCategoryName()
    {
        string categoryName = string.Empty;
        while (categoryName.Length < 2)
            categoryName = Faker.Commerce.Categories(1)[0];

        if (categoryName.Length > 255)
            categoryName = categoryName.Substring(0, 255);

        return categoryName;
    }

    public string GetValidCategoryDescription()
    {
        string categoryDescription = Faker.Commerce.ProductDescription();

        if (categoryDescription.Length > 10_000)
            categoryDescription = categoryDescription.Substring(0, 10_000);

        return categoryDescription;
    }

    public DomainEntity.Category GetValidCategory() => new(GetValidCategoryName(), "E.g: Soccer");
}

[CollectionDefinition(nameof(CategoryTestFixture))]
public class CategoryTestFixtureCollection : ICollectionFixture<CategoryTestFixture> { }