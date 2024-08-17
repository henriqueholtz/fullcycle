using CodeFlix.Catalog.Application.Exceptions;
using UseCaseDeleteCategory = CodeFlix.Catalog.Application.UseCases.Category.DeleteCategory;

namespace CodeFlix.Catalog.UnitTests.Application.DeleteCategory;

[Collection(nameof(DeleteCategoryFixture))]
public class DeleteCategoryTests
{
    private readonly DeleteCategoryFixture _fixture;

    public DeleteCategoryTests(DeleteCategoryFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(Success))]
    [Trait("Application", "DeleteCategory - Use Cases")]
    public async Task Success() {
        // Arrange
        var repositoryMock = _fixture.GetRepositoryMock();
        var uowMock = _fixture.GetUowMock();
        var validCategory = _fixture.GetValidCategory();
        repositoryMock.Setup(r => r.GetAsync(validCategory.Id, It.IsAny<CancellationToken>())).ReturnsAsync(validCategory);

        UseCaseDeleteCategory.DeleteCategoryInput input = new(validCategory.Id);
        UseCaseDeleteCategory.DeleteCategory useCase = new(repositoryMock.Object, uowMock.Object);

        // Act
        await useCase.Handle(input, CancellationToken.None);

        // Assert
        uowMock.Verify(r => r.CommitAsync(It.IsAny<CancellationToken>()), Times.Once());
        uowMock.VerifyNoOtherCalls();

        repositoryMock.Verify(r => r.GetAsync(validCategory.Id, It.IsAny<CancellationToken>()), Times.Once());
        repositoryMock.Verify(r => r.DeleteAsync(validCategory, It.IsAny<CancellationToken>()), Times.Once());
        repositoryMock.VerifyNoOtherCalls();
    }

    [Fact(DisplayName = nameof(ThrowWhenCategoryNotFound))]
    [Trait("Application", "DeleteCategory - Use Cases")]
    public async Task ThrowWhenCategoryNotFound() {
        // Arrange
        var repositoryMock = _fixture.GetRepositoryMock();
        var uowMock = _fixture.GetUowMock();
        var categoryId = Guid.NewGuid();
        repositoryMock.Setup(r => r.GetAsync(categoryId, It.IsAny<CancellationToken>())).ThrowsAsync(new NotFoundException($"Category '{categoryId}' not found."));

        UseCaseDeleteCategory.DeleteCategoryInput input = new(categoryId);
        UseCaseDeleteCategory.DeleteCategory useCase = new(repositoryMock.Object, uowMock.Object);
        var task = async () => await useCase.Handle(input, CancellationToken.None);

        // Act
        await task.Should().ThrowAsync<NotFoundException>();

        // Assert
        repositoryMock.Verify(r => r.GetAsync(categoryId, It.IsAny<CancellationToken>()), Times.Once());
        repositoryMock.VerifyNoOtherCalls();
        uowMock.VerifyNoOtherCalls();
    }
}
