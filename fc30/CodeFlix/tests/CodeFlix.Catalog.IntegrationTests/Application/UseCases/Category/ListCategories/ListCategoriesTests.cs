using CodeFlix.Catalog.Application.UseCases.Category.Common;
using CodeFlix.Catalog.Application.UseCases.Category.ListCategories;
using CodeFlix.Catalog.Domain.SeedWork.SearchableRepository;
using CodeFlix.Catalog.Infra.Data.EF;
using CodeFlix.Catalog.Infra.Data.EF.Repositories;
using ListCategoriesUseCase = CodeFlix.Catalog.Application.UseCases.Category.ListCategories;

namespace CodeFlix.Catalog.IntegrationTests.Application.UseCases.Category.ListCategories;

[Collection(nameof(ListCategoriesTestsFixture))]
public class ListCategoriesTests
{
    private readonly ListCategoriesTestsFixture _fixture;

    public ListCategoriesTests(ListCategoriesTestsFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(SearchReturnListAndTotal_Success))]
    [Trait("Integration/Application", "ListCategories - Use Cases")]
    public async Task SearchReturnListAndTotal_Success()
    {
        // Arrange
        CodeFlixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var categories = _fixture.GetValidCategories();
        await dbContext.AddRangeAsync(categories);
        await dbContext.SaveChangesAsync();
        var categoryRepository = new CategoryRepository(dbContext);
        var input = new ListCategoriesInput(1, 20);
        var useCase = new ListCategoriesUseCase.ListCategories(categoryRepository);

        // Act
        ListCategoriesOutput output = await useCase.Handle(input, CancellationToken.None);

        // Assert
        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.Page.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(categories.Count);
        output.Items.Should().HaveCount(categories.Count);

        foreach (CategoryModelOutput categoryOutput in output.Items)
        {
            var category = categories.Find(cat => cat.Id == categoryOutput.Id);
            category.Should().NotBeNull();
            categoryOutput.Should().NotBeNull();
            categoryOutput.Name.Should().Be(category!.Name);
            categoryOutput.Description.Should().Be(category.Description);
            categoryOutput.IsActive.Should().Be(category.IsActive);
            categoryOutput.CreatedAt.Should().Be(category.CreatedAt);
        }
    }


    [Fact(DisplayName = nameof(SearchReturnEmptyListAndTotal_Success))]
    [Trait("Integration/Application", "ListCategories - Use Cases")]
    public async Task SearchReturnEmptyListAndTotal_Success()
    {
        // Arrange
        CodeFlixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var categoryRepository = new CategoryRepository(dbContext);
        var input = new ListCategoriesInput(1, 20);
        var useCase = new ListCategoriesUseCase.ListCategories(categoryRepository);

        // Act
        ListCategoriesOutput output = await useCase.Handle(input, CancellationToken.None);

        // Assert
        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.Page.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(0);
        output.Items.Should().HaveCount(0);
    }

    [Theory(DisplayName = nameof(SearchReturnPaginatedListAndTotal_Success))]
    [Trait("Integration/Application", "ListCategories - Use Cases")]
    [InlineData(10, 1, 5, 5)]
    [InlineData(10, 2, 5, 5)]
    [InlineData(7, 2, 5, 2)]
    [InlineData(7, 3, 5, 0)]
    public async Task SearchReturnPaginatedListAndTotal_Success(int quantityToGenerate, int page, int perPage, int expectedQuantity)
    {
        // Arrange
        CodeFlixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var categories = _fixture.GetValidCategories(quantityToGenerate);
        await dbContext.AddRangeAsync(categories);
        await dbContext.SaveChangesAsync();
        var categoryRepository = new CategoryRepository(dbContext);
        var input = new ListCategoriesInput(page, perPage);
        var useCase = new ListCategoriesUseCase.ListCategories(categoryRepository);

        // Act
        ListCategoriesOutput output = await useCase.Handle(input, CancellationToken.None);

        // Assert
        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.Page.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(quantityToGenerate);
        output.Items.Should().HaveCount(expectedQuantity);

        foreach (CategoryModelOutput categoryOutput in output.Items)
        {
            var category = categories.Find(cat => cat.Id == categoryOutput.Id);
            category.Should().NotBeNull();
            categoryOutput.Should().NotBeNull();
            categoryOutput.Name.Should().Be(category!.Name);
            categoryOutput.Description.Should().Be(category.Description);
            categoryOutput.IsActive.Should().Be(category.IsActive);
            categoryOutput.CreatedAt.Should().Be(category.CreatedAt);
        }
    }

    [Theory(DisplayName = nameof(SearchByTextReturnPaginatedListAndTotal_Success))]
    [Trait("Integration/Application", "ListCategories - Use Cases")]
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
        var categories = _fixture.GetValidCategoriesWithFixedNames(new List<string> {
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
        await dbContext.AddRangeAsync(categories);
        await dbContext.SaveChangesAsync();
        var categoryRepository = new CategoryRepository(dbContext);
        var input = new ListCategoriesInput(page, perPage, search);
        var useCase = new ListCategoriesUseCase.ListCategories(categoryRepository);

        // Act
        ListCategoriesOutput output = await useCase.Handle(input, CancellationToken.None);

        // Assert
        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.Page.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(expectedQuantityTotal);
        output.Items.Should().HaveCount(expectedQuantityReturned);

        foreach (CategoryModelOutput categoryOutput in output.Items)
        {
            var category = categories.Find(cat => cat.Id == categoryOutput.Id);
            category.Should().NotBeNull();
            categoryOutput.Should().NotBeNull();
            categoryOutput.Name.Should().Be(category!.Name);
            categoryOutput.Description.Should().Be(category.Description);
            categoryOutput.IsActive.Should().Be(category.IsActive);
            categoryOutput.CreatedAt.Should().Be(category.CreatedAt);
        }
    }

    [Theory(DisplayName = nameof(SearchOrderedReturnPaginatedListAndTotal_Success))]
    [Trait("Integration/Application", "ListCategories - Use Cases")]
    [InlineData(SearchOrder.Asc, "name")]
    [InlineData(SearchOrder.Desc, "name")]
    [InlineData(SearchOrder.Asc, "id")]
    [InlineData(SearchOrder.Desc, "id")]
    [InlineData(SearchOrder.Asc, "createdAt")]
    [InlineData(SearchOrder.Desc, "createdAt")]
    [InlineData(SearchOrder.Desc, "invalid_field")]
    public async Task SearchOrderedReturnPaginatedListAndTotal_Success(SearchOrder order, string orderBy)
    {
        CodeFlixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var categories = _fixture.GetValidCategories();
        await dbContext.AddRangeAsync(categories);
        await dbContext.SaveChangesAsync();
        var categoryRepository = new CategoryRepository(dbContext);
        var input = new ListCategoriesInput(1, 20, sort: orderBy, dir: order);
        var useCase = new ListCategoriesUseCase.ListCategories(categoryRepository);

        // Act
        ListCategoriesOutput output = await useCase.Handle(input, CancellationToken.None);

        // Assert
        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.Page.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(categories.Count);
        output.Items.Should().HaveCount(categories.Count);

        var orderedCategories = _fixture.OrderCategories(categories, orderBy, order);
        for (int i = 0; i < categories.Count; i++)
        {
            var expectedOrderedCategory = orderedCategories[i];
            var outputCategory = output.Items[i];
            expectedOrderedCategory.Should().NotBeNull();
            outputCategory.Should().NotBeNull();
            outputCategory.Id.Should().Be(expectedOrderedCategory!.Id);
            outputCategory.Name.Should().Be(expectedOrderedCategory.Name);
            outputCategory.Description.Should().Be(expectedOrderedCategory.Description);
            outputCategory.IsActive.Should().Be(expectedOrderedCategory.IsActive);
            outputCategory.CreatedAt.Should().Be(expectedOrderedCategory.CreatedAt);
        }
    }
}
