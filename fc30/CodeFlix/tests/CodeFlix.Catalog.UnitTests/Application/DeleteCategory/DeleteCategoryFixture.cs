using CodeFlix.Catalog.UnitTests.Application.Common;

namespace CodeFlix.Catalog.UnitTests.Application.DeleteCategory;

[CollectionDefinition(nameof(DeleteCategoryFixture))]
public class DeleteCategoryFixtureCollection : ICollectionFixture<DeleteCategoryFixture> { }

public class DeleteCategoryFixture : CategoryUseCasesBaseFixture
{ }
