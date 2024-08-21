using CodeFlix.Catalog.Application.UseCases.Category.ListCategories;
using CodeFlix.Catalog.Domain.Entity;
using CodeFlix.Catalog.Domain.Repository;
using CodeFlix.Catalog.Domain.SeedWork.SearchableRepository;
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

    public ListCategoriesInput GetValidInput() {
        var random = new Random();
        return new ListCategoriesInput(
            page: random.Next(1, 10),
            perPage: random.Next(15, 100),
            search: Faker.Commerce.ProductName(),
            sort: Faker.Commerce.ProductName(),
            dir: random.Next(0, 10) > 5 ? SearchOrder.Asc : SearchOrder.Desc
        );
    }
}
