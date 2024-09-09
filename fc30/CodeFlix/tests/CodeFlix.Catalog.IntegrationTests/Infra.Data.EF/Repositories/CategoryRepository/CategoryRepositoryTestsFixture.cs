using CodeFlix.Catalog.Domain.Entity;
using CodeFlix.Catalog.Domain.SeedWork.SearchableRepository;
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
    public List<Category> GetValidCategoriesWithFixedNames(List<string> names)
    {
        return names.Select(name =>
        {
            var category = GetValidCategory();
            category.Update(name);
            return category;
        }).ToList();
    }
    public List<Category> GetValidCategories(int length = 10)
    {
        return Enumerable.Range(1, length)
            .Select(_ => GetValidCategory())
            .ToList();
    }

    public CodeFlixCatalogDbContext CreateDbContext(bool preserveData = false, bool randomDatabaseName = false)
    {
        string databaseName = "CodeFlix.Catalog.IntegrationTests";
        if (randomDatabaseName)
            databaseName += Guid.NewGuid().ToString();

        var dbContext = new CodeFlixCatalogDbContext(
            new DbContextOptionsBuilder<CodeFlixCatalogDbContext>()
                .UseInMemoryDatabase(databaseName)
                .Options
        );
        if (!preserveData)
            dbContext.Database.EnsureDeleted();
        return dbContext;
    }

    public List<Category> OrderCategories(List<Category> categories, string orderBy, SearchOrder order)
    {
        var orderedCategories = new List<Category>(categories);
        var ordered = (order, orderBy.ToLower()) switch
        {
            (SearchOrder.Asc, "name") => orderedCategories.OrderBy(c => c.Name),
            (SearchOrder.Desc, "name") => orderedCategories.OrderByDescending(c => c.Name),
            (SearchOrder.Asc, "id") => orderedCategories.OrderBy(c => c.Id),
            (SearchOrder.Desc, "id") => orderedCategories.OrderByDescending(c => c.Id),
            (SearchOrder.Asc, "createdat") => orderedCategories.OrderBy(c => c.CreatedAt),
            (SearchOrder.Desc, "createdat") => orderedCategories.OrderByDescending(c => c.CreatedAt),
            _ => orderedCategories.OrderBy(c => c.Name),
        };
        return ordered.ToList();
    }
}
