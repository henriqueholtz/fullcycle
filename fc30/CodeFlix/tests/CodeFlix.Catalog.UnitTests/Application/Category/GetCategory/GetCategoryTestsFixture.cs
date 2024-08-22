using CodeFlix.Catalog.UnitTests.Application.Category.Common;

namespace CodeFlix.Catalog.UnitTests.Application.Category.GetCategory;

[CollectionDefinition(nameof(GetCategoryTestsFixture))]
public class GetCategoryTestsFixtureCollection : ICollectionFixture<GetCategoryTestsFixture> { }

public class GetCategoryTestsFixture : CategoryUseCasesBaseFixture
{ }
