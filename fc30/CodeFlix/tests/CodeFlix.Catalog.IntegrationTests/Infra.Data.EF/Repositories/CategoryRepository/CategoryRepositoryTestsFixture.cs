using CodeFlix.Catalog.Domain.Entity;
using CodeFlix.Catalog.Infra.Data.EF;
using CodeFlix.Catalog.IntegrationTests.Base;
using Microsoft.EntityFrameworkCore;

namespace CodeFlix.Catalog.IntegrationTests.Infra.Data.EF.Repositories.CategoryRepository;

[CollectionDefinition(nameof(CategoryRepositoryTestsFixture))]
public class CategoryRepositoryTestsCollectionFixture : ICollectionFixture<CategoryRepositoryTestsFixture>
{ }

public class CategoryRepositoryTestsFixture : BaseFixture
{
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

    public bool GetRandomBoolean() => new Random().NextDouble() > 0.5;

    public Category GetValidCategory() => new(GetValidCategoryName(), GetValidCategoryDescription(), GetRandomBoolean());
    public CodeFlixCatalogDbContext CreateDbContext()
    {
        var dbContext = new CodeFlixCatalogDbContext(
            new DbContextOptionsBuilder<CodeFlixCatalogDbContext>()
                .UseInMemoryDatabase("CodeFlix.Catalog.IntegrationTests")
                .Options
        );
        return dbContext;
    }
}
