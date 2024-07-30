using CodeFlix.Catalog.Application;
using CodeFlix.Catalog.Domain.Entity;
using CodeFlix.Catalog.Domain.Repository;
using UseCases = CodeFlix.Catalog.Application.UseCases.Category.CreateCategory;

namespace CodeFlix.Catalog.UnitTests.Application.CreateCategory;

public class CreateCategoryTests
{
    [Fact(DisplayName = nameof(CreateCategorySuccess))]
    [Trait("Application", "CreateCategory - Use Cases")]
    public async Task CreateCategorySuccess()
    {
        // Arrange
        var repositoryMock = new Mock<ICategoryRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var useCase = new UseCases.CreateCategory(repositoryMock.Object, unitOfWorkMock.Object);
        var input = new UseCases.CreateCategoryInput("Category Name", "Description", true);

        // Act
        var output = await useCase.HandleAsync(input, CancellationToken.None);

        // Assert
        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().BeTrue();
        output.CreatedAt.Should().NotBeSameDateAs(default(DateTime));

        repositoryMock.Verify(rep => rep.InsertAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()), Times.Once);
        repositoryMock.VerifyNoOtherCalls();

        unitOfWorkMock.Verify(uow => uow.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        unitOfWorkMock.VerifyNoOtherCalls();
    }
}
