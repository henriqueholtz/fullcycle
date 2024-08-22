using CodeFlix.Catalog.UnitTests.Application.Category.Common;

namespace CodeFlix.Catalog.UnitTests.Application.Category.DeleteCategory;

[CollectionDefinition(nameof(DeleteCategoryFixture))]
public class DeleteCategoryFixtureCollection : ICollectionFixture<DeleteCategoryFixture> { }

public class DeleteCategoryFixture : CategoryUseCasesBaseFixture
{ }
