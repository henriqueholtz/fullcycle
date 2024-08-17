using UseCaseUpdateCategory = CodeFlix.Catalog.Application.UseCases.Category.UpdateCategory;

namespace CodeFlix.Catalog.UnitTests.Application.UpdateCategory;

[Collection(nameof(UpdateCategoryTestsFixture))]
public class UpdateCategoryTests : UpdateCategoryTestsFixture
{
    private readonly UpdateCategoryTestsFixture _fixture;

    public UpdateCategoryTests(UpdateCategoryTestsFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(Success))]
    [Trait("Application", "UpdateCategory - Use Cases")]
    public async Task Success() {
        // Arrange
        var repositoryMock = _fixture.GetRepositoryMock();
        var uowMock = _fixture.GetUowMock();
        var validCategory = _fixture.GetValidCategory();
        repositoryMock.Setup(r => r.GetAsync(validCategory.Id, It.IsAny<CancellationToken>())).ReturnsAsync(validCategory);

        UseCaseUpdateCategory.UpdateCategoryInput input = new(validCategory.Id, _fixture.GetValidCategoryName(), _fixture.GetValidCategoryDescription(), !validCategory.IsActive);
        UseCaseUpdateCategory.UpdateCategory useCase = new(repositoryMock.Object, uowMock.Object);

        // Act
        var output = await useCase.Handle(input, CancellationToken.None);

        // Assert
        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(input.IsActive);

        repositoryMock.Verify(r => r.GetAsync(validCategory.Id, It.IsAny<CancellationToken>()), Times.Once());
        repositoryMock.Verify(r => r.UpdateAsync(validCategory, It.IsAny<CancellationToken>()), Times.Once());
        repositoryMock.VerifyNoOtherCalls();

        uowMock.Verify(r => r.CommitAsync(It.IsAny<CancellationToken>()), Times.Once());
        uowMock.VerifyNoOtherCalls();
    }

}
