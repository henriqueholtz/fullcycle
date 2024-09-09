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
        CodeFlixCatalogDbContext dbContext = _fixture.CreateDbContext(false, true);
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

    [Theory(DisplayName = nameof(SearchReturnPaginatedListAndTotal_Success))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    [InlineData(10, 1, 5, 5)]
    [InlineData(10, 2, 5, 5)]
    [InlineData(7, 2, 5, 2)]
    [InlineData(7, 3, 5, 0)]
    public async Task SearchReturnPaginatedListAndTotal_Success(int quantityToGenerate, int page, int perPage, int expectedQuantity)
    {
        // Arrange
        CodeFlixCatalogDbContext dbContext = _fixture.CreateDbContext(false, true);
        var exampleCategories = _fixture.GetValidCategories(quantityToGenerate);
        await dbContext.AddRangeAsync(exampleCategories);
        await dbContext.SaveChangesAsync();
        var categoryRepository = new Repository.CategoryRepository(dbContext);
        var input = new SearchInput(page, perPage, "", "", SearchOrder.Asc);

        // Act
        var output = await categoryRepository.SearchAsync(input, CancellationToken.None);

        // Assert
        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.CurrentPage.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(quantityToGenerate);
        output.Items.Should().HaveCount(expectedQuantity);

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

    [Theory(DisplayName = nameof(SearchByTextReturnPaginatedListAndTotal_Success))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    [InlineData("Action", 1, 5, 1, 1)]
    [InlineData("Horror", 1, 5, 3, 3)]
    [InlineData("Horror", 2, 5, 0, 3)]
    [InlineData("Sci-fi", 1, 5, 4, 4)]
    [InlineData("Sci-fi", 1, 2, 2, 4)]
    [InlineData("Sci-fi", 2, 3, 1, 4)]
    [InlineData("Sci-fi Other", 1, 3, 0, 0)]
    [InlineData("Robots", 1, 5, 2, 2)]
    public async Task SearchByTextReturnPaginatedListAndTotal_Success(string search, int page, int perPage, int expectedQuantityReturned, int expectedQuantityTotal)
    {
        // Arrange
        CodeFlixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var exampleCategories = _fixture.GetValidCategoriesWithFixedNames(new List<string> {
            "Action",
            "Horror",
            "Horror - Robots",
            "Horror - Based on Real Facts",
            "Drama",
            "Sci-fi IA",
            "Sci-fi Space",
            "Sci-fi Robots",
            "Sci-fi Future",
        });
        await dbContext.AddRangeAsync(exampleCategories);
        await dbContext.SaveChangesAsync();
        var categoryRepository = new Repository.CategoryRepository(dbContext);
        var input = new SearchInput(page, perPage, search, "", SearchOrder.Asc);

        // Act
        var output = await categoryRepository.SearchAsync(input, CancellationToken.None);

        // Assert
        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.CurrentPage.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(expectedQuantityTotal);
        output.Items.Should().HaveCount(expectedQuantityReturned);

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

    [Theory(DisplayName = nameof(SearchOrderedReturnPaginatedListAndTotal_Success))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    [InlineData(SearchOrder.Asc, "name")]
    [InlineData(SearchOrder.Desc, "name")]
    [InlineData(SearchOrder.Asc, "id")]
    [InlineData(SearchOrder.Desc, "id")]
    [InlineData(SearchOrder.Asc, "createdAt")]
    [InlineData(SearchOrder.Desc, "createdAt")]
    [InlineData(SearchOrder.Desc, "invalid_field")]
    public async Task SearchOrderedReturnPaginatedListAndTotal_Success(SearchOrder order, string orderBy)
    {
        // Arrange
        CodeFlixCatalogDbContext dbContext = _fixture.CreateDbContext(false, true);
        var exampleCategories = _fixture.GetValidCategories();
        await dbContext.AddRangeAsync(exampleCategories);
        await dbContext.SaveChangesAsync();
        var categoryRepository = new Repository.CategoryRepository(dbContext);
        var input = new SearchInput(1, 20, "", orderBy, order);

        // Act
        var output = await categoryRepository.SearchAsync(input, CancellationToken.None);

        // Assert
        var orderedCategories = _fixture.OrderCategories(exampleCategories, orderBy, order);
        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.CurrentPage.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(exampleCategories.Count);
        output.Items.Should().HaveCount(exampleCategories.Count);

        for (int i = 0; i < exampleCategories.Count; i++)
        {
            var expectedCategory = orderedCategories[i];
            var outputCategory = output.Items[i];
            expectedCategory.Should().NotBeNull();
            outputCategory.Should().NotBeNull();
            outputCategory.Id.Should().Be(expectedCategory!.Id);
            outputCategory.Name.Should().Be(expectedCategory.Name);
            outputCategory.Description.Should().Be(expectedCategory.Description);
            outputCategory.IsActive.Should().Be(expectedCategory.IsActive);
            outputCategory.CreatedAt.Should().Be(expectedCategory.CreatedAt);
        }
    }
}