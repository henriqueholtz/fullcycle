using CodeFlix.Catalog.Domain.SeedWork.SearchableRepository;
using CodeFlix.Catalog.IntegrationTests.Application.UseCases.Category.Common;
using DomainEntity = CodeFlix.Catalog.Domain.Entity;

namespace CodeFlix.Catalog.IntegrationTests.Application.UseCases.Category.ListCategories;

[CollectionDefinition(nameof(ListCategoriesTestsFixture))]
public class ListCategoriesTestsFixtureCollection : ICollectionFixture<ListCategoriesTestsFixture> { }

public class ListCategoriesTestsFixture : CategoryUseCasesBaseFixture
{
    public List<DomainEntity.Category> GetValidCategoriesWithFixedNames(List<string> names)
    {
        return names.Select(name =>
        {
            var category = GetValidCategory();
            category.Update(name);
            return category;
        }).ToList();
    }

    public List<DomainEntity.Category> OrderCategories(List<DomainEntity.Category> categories, string orderBy, SearchOrder order)
    {
        var orderedCategories = new List<DomainEntity.Category>(categories);
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
