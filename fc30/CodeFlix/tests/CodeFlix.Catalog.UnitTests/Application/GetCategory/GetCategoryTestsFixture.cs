using CodeFlix.Catalog.UnitTests.Application.Common;

namespace CodeFlix.Catalog.UnitTests.Application.GetCategory;

[CollectionDefinition(nameof(GetCategoryTestsFixture))]
public class GetCategoryTestsFixtureCollection : ICollectionFixture<GetCategoryTestsFixture> { }

public class GetCategoryTestsFixture : CategoryUseCasesBaseFixture
{ }
