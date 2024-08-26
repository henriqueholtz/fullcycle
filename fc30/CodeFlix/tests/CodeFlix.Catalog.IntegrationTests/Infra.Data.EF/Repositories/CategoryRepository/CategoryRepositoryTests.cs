using CodeFlix.Catalog.Application.Exceptions;
using CodeFlix.Catalog.Infra.Data.EF;
using CodeFlix.Catalog.IntegrationTests.Base;
using Repository = CodeFlix.Catalog.Infra.Data.EF.Repositories;

namespace CodeFlix.Catalog.IntegrationTests.Infra.Data.EF.Repositories.CategoryRepository;

[Collection(nameof(CategoryRepositoryTestsFixture))]
public class CategoryRepositoryTests : BaseFixture
{
    private readonly CategoryRepositoryTestsFixture _fixture;

    public CategoryRepositoryTests(CategoryRepositoryTestsFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(InsertSuccess))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    public async Task InsertSuccess()
    {
        // Arrange
        CodeFlixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var category = _fixture.GetValidCategory();
        var categoryRepository = new Repository.CategoryRepository(dbContext);

        // Act
        await categoryRepository.InsertAsync(category, CancellationToken.None);

        // Assert
        var categoryDb = await dbContext.Categories.FindAsync(category.Id);
        categoryDb.Should().NotBeNull();
        categoryDb!.Name.Should().Be(category.Name);
        categoryDb.Description.Should().Be(category.Description);
        categoryDb.IsActive.Should().Be(category.IsActive);
        categoryDb.CreatedAt.Should().Be(category.CreatedAt);
    }

    [Fact(DisplayName = nameof(GetSuccess))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    public async Task GetSuccess()
    {
        // Arrange
        CodeFlixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var category = _fixture.GetValidCategory();
        var exampleCategories = _fixture.GetValidCategories();
        exampleCategories.Add(category);
        await dbContext.AddRangeAsync(exampleCategories);
        var categoryRepository = new Repository.CategoryRepository(dbContext);

        // Act
        var categoryDb = await categoryRepository.GetAsync(category.Id, CancellationToken.None);

        // Assert
        categoryDb.Should().NotBeNull();
        categoryDb!.Id.Should().Be(category.Id);
        categoryDb.Name.Should().Be(category.Name);
        categoryDb.Description.Should().Be(category.Description);
        categoryDb.IsActive.Should().Be(category.IsActive);
        categoryDb.CreatedAt.Should().Be(category.CreatedAt);
    }

    [Fact(DisplayName = nameof(GetThrowIfNotFound))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    public async Task GetThrowIfNotFound()
    {
        // Arrange
        CodeFlixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var notFoundCategoryId = Guid.NewGuid();
        var exampleCategories = _fixture.GetValidCategories();
        await dbContext.AddRangeAsync(exampleCategories);
        var categoryRepository = new Repository.CategoryRepository(dbContext);

        // Act
        var action = async () => await categoryRepository.GetAsync(notFoundCategoryId, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Category {notFoundCategoryId} not found.");
    }
}