using CodeFlix.Catalog.IntegrationTests.Application.UseCases.Category.Common;

namespace CodeFlix.Catalog.IntegrationTests.Application.UseCases.Category.DeleteCategory;

[CollectionDefinition(nameof(DeleteCategoryTestsFixture))]
public class DeleteCategoryTestsFixtureCollection : ICollectionFixture<DeleteCategoryTestsFixture> { }

public class DeleteCategoryTestsFixture : CategoryUseCasesBaseFixture
{ }
