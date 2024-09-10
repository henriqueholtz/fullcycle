using CodeFlix.Catalog.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using UnitOfWorkInfra = CodeFlix.Catalog.Infra.Data.EF;

namespace CodeFlix.Catalog.IntegrationTests.Infra.Data.EF.UnitOfWork;

[Collection(nameof(UnitOfWorkTestsFixture))]
public class UnitOfWorkTests
{
    private readonly UnitOfWorkTestsFixture _fixture;

    public UnitOfWorkTests(UnitOfWorkTestsFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(CommitSuccess))]
    [Trait("Integration/Infra.Data", "UnitOfWork - Persistence")]
    public async Task CommitSuccess()
    {
        // Arrange
        var dbContext = _fixture.CreateDbContext();
        var categories = _fixture.GetValidCategories();
        await dbContext.AddRangeAsync(categories);
        var ufw = new UnitOfWorkInfra.UnitOfWork(dbContext);

        // Act
        await ufw.CommitAsync(CancellationToken.None);

        // Assert
        List<Category> savedCategories = await (_fixture.CreateDbContext(true)).Categories.AsNoTracking().ToListAsync();
        savedCategories.Should().NotBeNullOrEmpty();
        savedCategories.Should().HaveCount(categories.Count);
    }

    [Fact(DisplayName = nameof(RollbackSuccess))]
    [Trait("Integration/Infra.Data", "UnitOfWork - Persistence")]
    public async Task RollbackSuccess()
    {
        // Arrange
        var dbContext = _fixture.CreateDbContext();
        var ufw = new UnitOfWorkInfra.UnitOfWork(dbContext);

        // Act
        var task = async () => await ufw.RollbackAsync(CancellationToken.None);

        // Assert
        await task.Should().NotThrowAsync<Exception>();
    }
}
