using CodeFlix.Catalog.Domain.Entity;
using CodeFlix.Catalog.Domain.Repository;
using CodeFlix.Catalog.UnitTests.Common;

namespace CodeFlix.Catalog.UnitTests.Application.ListCategories;

[CollectionDefinition(nameof(ListCategoriesTestsFixture))]
public class ListCategoriesTestsFixtureCollection : ICollectionFixture<ListCategoriesTestsFixture> {}

public class ListCategoriesTestsFixture : BaseFixture
{ 
    public Mock<ICategoryRepository> GetRepositoryMock() => new();
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

    public bool GetRandomBoolean() => (new Random()).NextDouble() > 0.5;

    public Category GetValidCategory() => new(GetValidCategoryName(), GetValidCategoryDescription(), GetRandomBoolean());
    
    public List<Category> GetCategories(int length = 10) {
        var list = new List<Category>();
        for (int i = 0; i < length; i++) {
            list.Add(GetValidCategory());
        }
        return list;
    }
}
