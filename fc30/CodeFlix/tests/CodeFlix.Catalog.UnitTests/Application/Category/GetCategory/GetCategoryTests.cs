using UseCaseGetCategory = CodeFlix.Catalog.Application.UseCases.Category.GetCategory;
using UseCaseCommon = CodeFlix.Catalog.Application.UseCases.Category.Common;
using CodeFlix.Catalog.Application.Exceptions;

namespace CodeFlix.Catalog.UnitTests.Application.Category.GetCategory;

[Collection(nameof(GetCategoryTestsFixture))]
public class GetCategoryTests
{
    private readonly GetCategoryTestsFixture _fixture;

    public GetCategoryTests(GetCategoryTestsFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(GetSuccessfully))]
    [Trait("Application", "GetCategory - Use Cases")]
    public async Task GetSuccessfully()
    {
        // Arrange
        var repositoryMock = _fixture.GetRepositoryMock();
        var exampleCategory = _fixture.GetValidCategory();
        repositoryMock.Setup(r => r.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(exampleCategory);
        var input = new UseCaseGetCategory.GetCategoryInput(exampleCategory.Id);
        var useCase = new UseCaseGetCategory.GetCategory(repositoryMock.Object);

        // Act
        UseCaseCommon.CategoryModelOutput output = await useCase.Handle(input, CancellationToken.None);

        // Assert
        output.Should().NotBeNull();
        output.Id.Should().Be(exampleCategory.Id);
        output.Name.Should().Be(exampleCategory.Name);
        output.Description.Should().Be(exampleCategory.Description);
        output.IsActive.Should().Be(exampleCategory.IsActive);
        output.CreatedAt.Should().Be(exampleCategory.CreatedAt);

        repositoryMock.Verify(rep => rep.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        repositoryMock.VerifyNoOtherCalls();
    }

    [Fact(DisplayName = nameof(NotFoundExceptionWhenCategoryDoesNotExists))]
    [Trait("Application", "GetCategory - Use Cases")]
    public async Task NotFoundExceptionWhenCategoryDoesNotExists()
    {
        // Arrange
        var repositoryMock = _fixture.GetRepositoryMock();
        var exampleGuid = Guid.NewGuid();
        repositoryMock.Setup(r => r.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ThrowsAsync(new NotFoundException($"Category '{exampleGuid}' not found"));
        var input = new UseCaseGetCategory.GetCategoryInput(exampleGuid);
        var useCase = new UseCaseGetCategory.GetCategory(repositoryMock.Object);

        // Act
        var task = async () => await useCase.Handle(input, CancellationToken.None);

        // Assert
        await task.Should().ThrowAsync<NotFoundException>();

        repositoryMock.Verify(rep => rep.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        repositoryMock.VerifyNoOtherCalls();
    }
}
