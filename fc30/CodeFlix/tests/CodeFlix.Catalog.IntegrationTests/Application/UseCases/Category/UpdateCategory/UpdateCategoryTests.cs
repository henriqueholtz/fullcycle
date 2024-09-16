using CodeFlix.Catalog.Application.Exceptions;
using CodeFlix.Catalog.Application.UseCases.Category.UpdateCategory;
using CodeFlix.Catalog.Domain.Exceptions;
using CodeFlix.Catalog.Infra.Data.EF;
using CodeFlix.Catalog.Infra.Data.EF.Repositories;
using Microsoft.EntityFrameworkCore;
using DomainEntity = CodeFlix.Catalog.Domain.Entity;
using UseCaseUpdateCategory = CodeFlix.Catalog.Application.UseCases.Category.UpdateCategory;
namespace CodeFlix.Catalog.IntegrationTests.Application.UseCases.Category.UpdateCategory;

[Collection(nameof(UpdateCategoryTestsFixture))]
public class UpdateCategoryTests
{
    private readonly UpdateCategoryTestsFixture _fixture;

    public UpdateCategoryTests(UpdateCategoryTestsFixture fixture)
    {
        _fixture = fixture;
    }

    [Theory(DisplayName = nameof(FullUpdateShouldBeSuccessful))]
    [Trait("Integration/Application", "UpdateCategory - Use Cases")]
    [MemberData(nameof(UpdateCategoryTestsGenerator.GetCategoriesToUpdate), parameters: 5, MemberType = typeof(UpdateCategoryTestsGenerator))]
    public async Task FullUpdateShouldBeSuccessful(DomainEntity.Category category, UpdateCategoryInput input)
    {
        // Arrange
        var dbContext = _fixture.CreateDbContext();
        await dbContext.AddRangeAsync(_fixture.GetValidCategories());
        var trackingInfo = await dbContext.AddAsync(category);
        await dbContext.SaveChangesAsync();
        trackingInfo.State = EntityState.Detached;
        var repository = new CategoryRepository(dbContext);
        var uow = new UnitOfWork(dbContext);
        UseCaseUpdateCategory.UpdateCategory useCase = new(repository, uow);

        // Act
        var output = await useCase.Handle(input, CancellationToken.None);

        // Assert
        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description); output.IsActive.Should().Be((bool)input.IsActive!);

        var categoryDb = await (_fixture.CreateDbContext(true)).Categories.FindAsync(output.Id);
        categoryDb.Should().NotBeNull();
        categoryDb!.Name.Should().Be(input.Name);
        categoryDb.Description.Should().Be(input.Description);
        categoryDb.IsActive.Should().Be((bool)input.IsActive!);
        categoryDb.CreatedAt.Should().Be(output.CreatedAt);
    }

    [Theory(DisplayName = nameof(UpdateWithoutIsActiveShouldBeSuccessful))]
    [Trait("Integration/Application", "UpdateCategory - Use Cases")]
    [MemberData(nameof(UpdateCategoryTestsGenerator.GetCategoriesToUpdate), parameters: 5, MemberType = typeof(UpdateCategoryTestsGenerator))]
    public async Task UpdateWithoutIsActiveShouldBeSuccessful(DomainEntity.Category category, UpdateCategoryInput inputBase)
    {
        // Arrange
        var input = new UpdateCategoryInput(inputBase.Id, inputBase.Name, inputBase.Description);
        var dbContext = _fixture.CreateDbContext();
        await dbContext.AddRangeAsync(_fixture.GetValidCategories());
        var trackingInfo = await dbContext.AddAsync(category);
        await dbContext.SaveChangesAsync();
        trackingInfo.State = EntityState.Detached;
        var repository = new CategoryRepository(dbContext);
        var uow = new UnitOfWork(dbContext);
        UseCaseUpdateCategory.UpdateCategory useCase = new(repository, uow);

        // Act
        var output = await useCase.Handle(input, CancellationToken.None);

        // Assert
        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(category.IsActive);

        var categoryDb = await (_fixture.CreateDbContext(true)).Categories.FindAsync(output.Id);
        categoryDb.Should().NotBeNull();
        categoryDb!.Name.Should().Be(input.Name);
        categoryDb.Description.Should().Be(input.Description);
        categoryDb.IsActive.Should().Be(category.IsActive!);
        categoryDb.CreatedAt.Should().Be(output.CreatedAt);
    }

    [Theory(DisplayName = nameof(UpdateOnlyNameShouldBeSuccessful))]
    [Trait("Integration/Application", "UpdateCategory - Use Cases")]
    [MemberData(nameof(UpdateCategoryTestsGenerator.GetCategoriesToUpdate), parameters: 5, MemberType = typeof(UpdateCategoryTestsGenerator))]
    public async Task UpdateOnlyNameShouldBeSuccessful(DomainEntity.Category category, UpdateCategoryInput inputBase)
    {
        // Arrange
        var input = new UpdateCategoryInput(inputBase.Id, inputBase.Name);
        var dbContext = _fixture.CreateDbContext();
        await dbContext.AddRangeAsync(_fixture.GetValidCategories());
        var trackingInfo = await dbContext.AddAsync(category);
        await dbContext.SaveChangesAsync();
        trackingInfo.State = EntityState.Detached;
        var repository = new CategoryRepository(dbContext);
        var uow = new UnitOfWork(dbContext);
        UseCaseUpdateCategory.UpdateCategory useCase = new(repository, uow);

        // Act
        var output = await useCase.Handle(input, CancellationToken.None);

        // Assert
        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(category.Description);
        output.IsActive.Should().Be(category.IsActive);

        var categoryDb = await (_fixture.CreateDbContext(true)).Categories.FindAsync(output.Id);
        categoryDb.Should().NotBeNull();
        categoryDb!.Name.Should().Be(input.Name);
        categoryDb.Description.Should().Be(category.Description);
        categoryDb.IsActive.Should().Be(category.IsActive!);
        categoryDb.CreatedAt.Should().Be(output.CreatedAt);
    }

    [Fact(DisplayName = nameof(UpdateThrowsWhenNotFoundCategory))]
    [Trait("Integration/Application", "UpdateCategory - Use Cases")]
    public async Task UpdateThrowsWhenNotFoundCategory()
    {
        // Arrange
        var input = _fixture.GetValidInput();
        var dbContext = _fixture.CreateDbContext();
        await dbContext.AddRangeAsync(_fixture.GetValidCategories());
        await dbContext.SaveChangesAsync();
        var repository = new CategoryRepository(dbContext);
        var uow = new UnitOfWork(dbContext);
        UseCaseUpdateCategory.UpdateCategory useCase = new(repository, uow);

        // Act
        var task = async () => await useCase.Handle(input, CancellationToken.None);

        // Assert
        await task.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Category {input.Id} not found.");
    }

    [Theory(DisplayName = nameof(UpdateThrowsWhenCannotInstantiateACategory))]
    [Trait("Integration/Application", "UpdateCategory - Use Cases")]
    [MemberData(nameof(UpdateCategoryTestsGenerator.GetInvalidInputs), parameters: 5, MemberType = typeof(UpdateCategoryTestsGenerator))]
    public async Task UpdateThrowsWhenCannotInstantiateACategory(UpdateCategoryInput input, string expectedExceptionMessage)
    {
        // Arrange
        var dbContext = _fixture.CreateDbContext();
        var categories = _fixture.GetValidCategories();
        await dbContext.AddRangeAsync(categories);
        await dbContext.SaveChangesAsync();
        var repository = new CategoryRepository(dbContext);
        var uow = new UnitOfWork(dbContext);
        UseCaseUpdateCategory.UpdateCategory useCase = new(repository, uow);
        input.Id = categories[0].Id;

        // Act
        var task = async () => await useCase.Handle(input, CancellationToken.None);

        // Assert
        await task.Should().ThrowAsync<EntityValidationException>()
            .WithMessage(expectedExceptionMessage);
    }
}
