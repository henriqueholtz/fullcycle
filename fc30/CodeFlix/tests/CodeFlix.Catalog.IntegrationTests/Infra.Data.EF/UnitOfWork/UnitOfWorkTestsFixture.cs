using CodeFlix.Catalog.Domain.Entity;
using CodeFlix.Catalog.IntegrationTests.Base;

namespace CodeFlix.Catalog.IntegrationTests.Infra.Data.EF.UnitOfWork;

[CollectionDefinition(nameof(UnitOfWorkTestsFixture))]
public class UnitOfWorkTestsFixtureCollection : ICollectionFixture<UnitOfWorkTestsFixture> { }

public class UnitOfWorkTestsFixture : BaseFixture
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

    public List<Category> GetValidCategories(int length = 10)
    {
        return Enumerable.Range(1, length)
            .Select(_ => GetValidCategory())
            .ToList();
    }
}
