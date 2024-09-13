using CodeFlix.Catalog.IntegrationTests.Application.UseCases.Category.Common;

namespace CodeFlix.Catalog.IntegrationTests.Application.UseCases.Category.GetCategory;

[CollectionDefinition(nameof(GetCategoryTestsFixture))]
public class GetCategoryTestsFixtureCollection : ICollectionFixture<GetCategoryTestsFixture> { }

public class GetCategoryTestsFixture : CategoryUseCasesBaseFixture
{ }
