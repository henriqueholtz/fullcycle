using CodeFlix.Catalog.Domain.Exceptions;
using CodeFlix.Catalog.Infra.Data.EF;
using CodeFlix.Catalog.Infra.Data.EF.Repositories;
using Microsoft.EntityFrameworkCore;
using UseCaseCreateCategory = CodeFlix.Catalog.Application.UseCases.Category.CreateCategory;

namespace CodeFlix.Catalog.IntegrationTests.Application.UseCases.Category.CreateCategory;

[Collection(nameof(CreateCategoryTestsFixture))]
public class CreateCategoryTests : CreateCategoryTestsFixture
{
    private readonly CreateCategoryTestsFixture _fixture;

    public CreateCategoryTests(CreateCategoryTestsFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(CreateSuccess))]
    [Trait("Integration/Application", "GetCategory - Use Cases")]
    public async Task CreateSuccess()
    {
        // Arranje
        var dbContext = _fixture.CreateDbContext();
        var repository = new CategoryRepository(dbContext);
        var uow = new UnitOfWork(dbContext);
        var input = _fixture.CreateValidInput();
        var useCase = new UseCaseCreateCategory.CreateCategory(repository, uow);

        // Act
        var output = await useCase.Handle(input, CancellationToken.None);

        // Assert
        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(input.IsActive);
        output.CreatedAt.Should().NotBeSameDateAs(default);

        var categoryDb = await (_fixture.CreateDbContext(true)).Categories.FindAsync(output.Id);
        categoryDb.Should().NotBeNull();
        categoryDb!.Name.Should().Be(input.Name);
        categoryDb.Description.Should().Be(input.Description);
        categoryDb.IsActive.Should().Be(input.IsActive);
        categoryDb.CreatedAt.Should().Be(output.CreatedAt);
    }

    [Fact(DisplayName = nameof(CreateWithOnlyNameSuccess))]
    [Trait("Integration/Application", "GetCategory - Use Cases")]
    public async Task CreateWithOnlyNameSuccess()
    {
        // Arranje
        var dbContext = _fixture.CreateDbContext();
        var repository = new CategoryRepository(dbContext);
        var uow = new UnitOfWork(dbContext);
        var input = new UseCaseCreateCategory.CreateCategoryInput(_fixture.GetValidCategoryName());
        var useCase = new UseCaseCreateCategory.CreateCategory(repository, uow);

        // Act
        var output = await useCase.Handle(input, CancellationToken.None);

        // Assert
        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.Name.Should().Be(input.Name);
        output.Description.Should().BeEmpty();
        output.IsActive.Should().BeTrue();
        output.CreatedAt.Should().NotBeSameDateAs(default);

        var categoryDb = await (_fixture.CreateDbContext(true)).Categories.FindAsync(output.Id);
        categoryDb.Should().NotBeNull();
        categoryDb!.Name.Should().Be(input.Name);
        categoryDb.Description.Should().BeEmpty();
        categoryDb.IsActive.Should().BeTrue();
        categoryDb.CreatedAt.Should().Be(output.CreatedAt);
    }

    [Fact(DisplayName = nameof(CreateWithOnlyNameAndDescriptionSuccess))]
    [Trait("Integration/Application", "GetCategory - Use Cases")]
    public async Task CreateWithOnlyNameAndDescriptionSuccess()
    {
        // Arranje
        var dbContext = _fixture.CreateDbContext();
        var repository = new CategoryRepository(dbContext);
        var uow = new UnitOfWork(dbContext);
        var category = _fixture.GetValidCategory();
        var input = new UseCaseCreateCategory.CreateCategoryInput(category.Name, category.Description);
        var useCase = new UseCaseCreateCategory.CreateCategory(repository, uow);

        // Act
        var output = await useCase.Handle(input, CancellationToken.None);

        // Assert
        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().BeTrue();
        output.CreatedAt.Should().NotBeSameDateAs(default);

        var categoryDb = await (_fixture.CreateDbContext(true)).Categories.FindAsync(output.Id);
        categoryDb.Should().NotBeNull();
        categoryDb!.Name.Should().Be(input.Name);
        categoryDb.Description.Should().Be(input.Description);
        categoryDb.IsActive.Should().BeTrue();
        categoryDb.CreatedAt.Should().Be(output.CreatedAt);
    }

    [Theory(DisplayName = nameof(ThrowWhenCanNotInstantiateCategory))]
    [Trait("Integration/Application", "GetCategory - Use Cases")]
    [MemberData(nameof(CreateCategoryTestsDataGenerator.GetInvalidInputs), parameters: 4, MemberType = typeof(CreateCategoryTestsDataGenerator))]
    public async Task ThrowWhenCanNotInstantiateCategory(UseCaseCreateCategory.CreateCategoryInput input, string exceptionMessage)
    {
        // Arranje
        var dbContext = _fixture.CreateDbContext();
        var repository = new CategoryRepository(dbContext);
        var uow = new UnitOfWork(dbContext);
        var category = _fixture.GetValidCategory();
        var useCase = new UseCaseCreateCategory.CreateCategory(repository, uow);

        // Act
        var task = async () => await useCase.Handle(input, CancellationToken.None);

        // Assert
        await task.Should().ThrowAsync<EntityValidationException>().WithMessage(exceptionMessage);
        var categoriesDb = _fixture.CreateDbContext(true).Categories.AsNoTracking().ToList();
        categoriesDb.Should().BeEmpty();
    }
}
