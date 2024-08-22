using CodeFlix.Catalog.Application.UseCases.Category.ListCategories;
using CodeFlix.Catalog.Domain.Entity;
using CodeFlix.Catalog.Domain.SeedWork.SearchableRepository;
using CodeFlix.Catalog.UnitTests.Application.Common;

namespace CodeFlix.Catalog.UnitTests.Application.ListCategories;

[CollectionDefinition(nameof(ListCategoriesTestsFixture))]
public class ListCategoriesTestsFixtureCollection : ICollectionFixture<ListCategoriesTestsFixture> {}

public class ListCategoriesTestsFixture : CategoryUseCasesBaseFixture
{ 
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
