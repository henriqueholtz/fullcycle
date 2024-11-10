using CodeFlix.Catalog.Application.UseCases.Category.CreateCategory;
using CodeFlix.Catalog.EndToEndTests.Api.Category.Common;

namespace CodeFlix.Catalog.EndToEndTests.Api.Category.CreateCategory;


[CollectionDefinition(nameof(CreateCategoryApiTestsFixture))]
public class CreateCategoryApiTestsFixtureCollection : ICollectionFixture<CreateCategoryApiTestsFixture> { }

public class CreateCategoryApiTestsFixture : CategoryApiBaseTestsFixture
{
    public CreateCategoryInput GetValidInput() => new(
        GetValidCategoryName(),
        GetValidCategoryDescription(),
        GetRandomBoolean()
    );
}
