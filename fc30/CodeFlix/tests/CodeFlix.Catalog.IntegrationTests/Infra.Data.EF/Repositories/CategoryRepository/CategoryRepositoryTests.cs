using CodeFlix.Catalog.Application.Exceptions;
using CodeFlix.Catalog.Domain.Entity;
using CodeFlix.Catalog.Domain.SeedWork.SearchableRepository;
using CodeFlix.Catalog.Infra.Data.EF;
using Repository = CodeFlix.Catalog.Infra.Data.EF.Repositories;

namespace CodeFlix.Catalog.IntegrationTests.Infra.Data.EF.Repositories.CategoryRepository;

[Collection(nameof(CategoryRepositoryTestsFixture))]
public class CategoryRepositoryTests
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
        await dbContext.SaveChangesAsync();

        // Assert
        var categoryDb = await (_fixture.CreateDbContext(true)).Categories.FindAsync(category.Id);
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
        await dbContext.SaveChangesAsync();
        var categoryRepository = new Repository.CategoryRepository(_fixture.CreateDbContext(true));

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
        await dbContext.SaveChangesAsync();
        var categoryRepository = new Repository.CategoryRepository(dbContext);

        // Act
        var action = async () => await categoryRepository.GetAsync(notFoundCategoryId, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Category {notFoundCategoryId} not found.");
    }

    [Fact(DisplayName = nameof(UpdateSuccess))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    public async Task UpdateSuccess()
    {
        // Arrange
        CodeFlixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var category = _fixture.GetValidCategory();
        var newCategory = _fixture.GetValidCategory();
        var exampleCategories = _fixture.GetValidCategories();
        exampleCategories.Add(category);
        await dbContext.AddRangeAsync(exampleCategories);
        await dbContext.SaveChangesAsync();
        var categoryRepository = new Repository.CategoryRepository(dbContext);
        category.Update(newCategory.Name, newCategory.Description);

        // Act
        await categoryRepository.UpdateAsync(category, CancellationToken.None);
        await dbContext.SaveChangesAsync();

        // Assert
        var categoryDb = await (_fixture.CreateDbContext(true)).Categories.FindAsync(category.Id);
        categoryDb.Should().NotBeNull();
        categoryDb!.Id.Should().Be(category.Id);
        categoryDb.Name.Should().Be(newCategory.Name);
        categoryDb.Description.Should().Be(newCategory.Description);
        categoryDb.IsActive.Should().Be(category.IsActive);
        categoryDb.CreatedAt.Should().Be(category.CreatedAt);
    }

    [Fact(DisplayName = nameof(DeleteSuccess))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    public async Task DeleteSuccess()
    {
        // Arrange
        CodeFlixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var category = _fixture.GetValidCategory();
        var exampleCategories = _fixture.GetValidCategories();
        exampleCategories.Add(category);
        await dbContext.AddRangeAsync(exampleCategories);
        await dbContext.SaveChangesAsync();
        var categoryRepository = new Repository.CategoryRepository(dbContext);

        // Act
        await categoryRepository.DeleteAsync(category, CancellationToken.None);
        await dbContext.SaveChangesAsync();

        // Assert
        var categoryDb = await (_fixture.CreateDbContext(true)).Categories.FindAsync(category.Id);
        categoryDb.Should().BeNull();
    }

    [Fact(DisplayName = nameof(SearchReturnListAndTotal_Success))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    public async Task SearchReturnListAndTotal_Success()
    {
        // Arrange
        CodeFlixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var exampleCategories = _fixture.GetValidCategories();
        await dbContext.AddRangeAsync(exampleCategories);
        await dbContext.SaveChangesAsync();
        var categoryRepository = new Repository.CategoryRepository(dbContext);
        var input = new SearchInput(1, 20, "", "", SearchOrder.Asc);

        // Act
        var output = await categoryRepository.SearchAsync(input, CancellationToken.None);

        // Assert
        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.CurrentPage.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(exampleCategories.Count);
        output.Items.Should().HaveCount(exampleCategories.Count);

        foreach (Category outputCategory in output.Items)
        {
            var exampleCategory = exampleCategories.Find(cat => cat.Id == outputCategory.Id);
            exampleCategory.Should().NotBeNull();
            outputCategory.Should().NotBeNull();
            outputCategory.Name.Should().Be(exampleCategory!.Name);
            outputCategory.Description.Should().Be(exampleCategory.Description);
            outputCategory.IsActive.Should().Be(exampleCategory.IsActive);
            outputCategory.CreatedAt.Should().Be(exampleCategory.CreatedAt);
        }
    }

    [Fact(DisplayName = nameof(SearchReturnEmptyListAndTotal_Success))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    public async Task SearchReturnEmptyListAndTotal_Success()
    {
        // Arrange
        CodeFlixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var categoryRepository = new Repository.CategoryRepository(dbContext);
        var input = new SearchInput(1, 20, "", "", SearchOrder.Asc);

        // Act
        var output = await categoryRepository.SearchAsync(input, CancellationToken.None);

        // Assert
        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.CurrentPage.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(0);
        output.Items.Should().HaveCount(0);
    }
}