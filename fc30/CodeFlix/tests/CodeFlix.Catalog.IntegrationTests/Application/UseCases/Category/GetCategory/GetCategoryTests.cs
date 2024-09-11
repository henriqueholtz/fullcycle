using CodeFlix.Catalog.Application.Exceptions;
using CodeFlix.Catalog.Application.UseCases.Category.Common;
using CodeFlix.Catalog.Application.UseCases.Category.GetCategory;
using CodeFlix.Catalog.Infra.Data.EF.Repositories;
using GetCategoryUseCase = CodeFlix.Catalog.Application.UseCases.Category.GetCategory;

namespace CodeFlix.Catalog.IntegrationTests.Application.UseCases.Category.GetCategory;

[Collection(nameof(GetCategoryTestsFixture))]
public class GetCategoryTests
{
    private readonly GetCategoryTestsFixture _fixture;

    public GetCategoryTests(GetCategoryTestsFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(Success))]
    [Trait("Integration/Application", "GetCategory - Use Cases")]
    public async Task Success()
    {
        // Arranje
        var category = _fixture.GetValidCategory();
        var dbContext = _fixture.CreateDbContext();
        await dbContext.AddAsync(category);
        await dbContext.SaveChangesAsync();
        var repository = new CategoryRepository(dbContext);
        var useCase = new GetCategoryUseCase.GetCategory(repository);
        var input = new GetCategoryInput(category.Id);

        // Act
        CategoryModelOutput output = await useCase.Handle(input, CancellationToken.None);

        // Assert
        output.Should().NotBeNull();
        output.Name.Should().Be(category.Name);
        output.Description.Should().Be(category.Description);
        output.IsActive.Should().Be(category.IsActive);
        output.Id.Should().Be(category.Id);
        output.CreatedAt.Should().Be(category.CreatedAt);
    }

    [Fact(DisplayName = nameof(ThrowNotFoundExceptionIfNotExists))]
    [Trait("Integration/Application", "GetCategory - Use Cases")]
    public async Task ThrowNotFoundExceptionIfNotExists()
    {
        // Arranje
        var dbContext = _fixture.CreateDbContext();
        var repository = new CategoryRepository(dbContext);
        var useCase = new GetCategoryUseCase.GetCategory(repository);
        var categoryId = Guid.NewGuid();
        var input = new GetCategoryInput(categoryId);

        // Act
        var task = async () => await useCase.Handle(input, CancellationToken.None);

        // Assert
        await task.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Category {categoryId} not found.");
    }
}
