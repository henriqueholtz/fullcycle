using CodeFlix.Catalog.Application.UseCases.Category.CreateCategory;
using CodeFlix.Catalog.Domain.Entity;
using UseCases = CodeFlix.Catalog.Application.UseCases.Category.CreateCategory;

namespace CodeFlix.Catalog.UnitTests.Application.CreateCategory;

[Collection(nameof(CreateCategoryTestsFixture))]
public class CreateCategoryTests
{
    private readonly CreateCategoryTestsFixture _fixture;
    public CreateCategoryTests(CreateCategoryTestsFixture createCategoryTestsFixture)
    {
        _fixture = createCategoryTestsFixture;
    }

    [Fact(DisplayName = nameof(CreateCategorySuccess))]
    [Trait("Application", "CreateCategory - Use Cases")]
    public async Task CreateCategorySuccess()
    {
        // Arrange
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUowMock();
        var useCase = new UseCases.CreateCategory(repositoryMock.Object, unitOfWorkMock.Object);
        CreateCategoryInput input = _fixture.GetValidInput();

        // Act
        var output = await useCase.HandleAsync(input, CancellationToken.None);

        // Assert
        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(input.IsActive);
        output.CreatedAt.Should().NotBeSameDateAs(default(DateTime));

        repositoryMock.Verify(rep => rep.InsertAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()), Times.Once);
        repositoryMock.VerifyNoOtherCalls();

        unitOfWorkMock.Verify(uow => uow.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        unitOfWorkMock.VerifyNoOtherCalls();
    }
}
