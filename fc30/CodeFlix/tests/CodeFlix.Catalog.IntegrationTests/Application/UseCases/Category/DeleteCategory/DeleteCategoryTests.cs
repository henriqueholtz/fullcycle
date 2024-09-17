using CodeFlix.Catalog.Application.Exceptions;
using CodeFlix.Catalog.Infra.Data.EF;
using CodeFlix.Catalog.Infra.Data.EF.Repositories;
using Microsoft.EntityFrameworkCore;
using UseCaseDeleteCategory = CodeFlix.Catalog.Application.UseCases.Category.DeleteCategory;

namespace CodeFlix.Catalog.IntegrationTests.Application.UseCases.Category.DeleteCategory;

[Collection(nameof(DeleteCategoryTestsFixture))]
public class DeleteCategoryTests
{
    private readonly DeleteCategoryTestsFixture _fixture;

    public DeleteCategoryTests(DeleteCategoryTestsFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(Success))]
    [Trait("Infraestructure/Application", "DeleteCategory - Use Cases")]
    public async Task Success()
    {
        // Arrange
        var dbContext = _fixture.CreateDbContext();
        var categories = _fixture.GetValidCategories();
        var categoryToRemove = _fixture.GetValidCategory();
        var trackingInfo = await dbContext.AddAsync(categoryToRemove);
        await dbContext.AddRangeAsync(categories);
        await dbContext.SaveChangesAsync();
        trackingInfo.State = EntityState.Detached;

        var repository = new CategoryRepository(dbContext);
        var uow = new UnitOfWork(dbContext);
        UseCaseDeleteCategory.DeleteCategoryInput input = new(categoryToRemove.Id);
        UseCaseDeleteCategory.DeleteCategory useCase = new(repository, uow);

        // Act
        await useCase.Handle(input, CancellationToken.None);

        // Assert
        var assertDbContext = _fixture.CreateDbContext(true);
        var categoryDb = await assertDbContext.Categories.FindAsync(categoryToRemove.Id);
        categoryDb.Should().BeNull();
        assertDbContext.Categories.Should().HaveCount(categories.Count);
    }

    [Fact(DisplayName = nameof(ThrowWhenCategoryNotFound))]
    [Trait("Infraestructure/Application", "DeleteCategory - Use Cases")]
    public async Task ThrowWhenCategoryNotFound()
    {
        // Arrange
        var dbContext = _fixture.CreateDbContext();
        var categories = _fixture.GetValidCategories();
        Guid categoryIdToRemove = Guid.NewGuid();
        await dbContext.AddRangeAsync(categories);
        await dbContext.SaveChangesAsync();

        var repository = new CategoryRepository(dbContext);
        var uow = new UnitOfWork(dbContext);
        UseCaseDeleteCategory.DeleteCategoryInput input = new(categoryIdToRemove);
        UseCaseDeleteCategory.DeleteCategory useCase = new(repository, uow);

        // Act
        var task = async () => await useCase.Handle(input, CancellationToken.None);

        // Assert
        await task.Should().ThrowAsync<NotFoundException>().WithMessage($"Category {categoryIdToRemove} not found.");
    }
}
