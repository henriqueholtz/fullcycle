using CodeFlix.Catalog.UnitTests.Application.GetCategory;
using UseCaseGetCategory = CodeFlix.Catalog.Application.UseCases.Category.GetCategory;

namespace CodeFlix.Catalog.UnitTests;

[Collection(nameof(GetCategoryTestsFixture))]
public class GetCategoryTests
{
    private readonly GetCategoryTestsFixture _fixture;

    public GetCategoryTests(GetCategoryTestsFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = "")]
    [Trait("Application", "GetCategory - Use Cases")]
    public async Task GetSuccessfully() {
        // Arranje
        var repositoryMock = _fixture.GetRepositoryMock();
        var exampleCategory = _fixture.GetValidCategory();
        repositoryMock.Setup(r => r.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(exampleCategory);
        var input = new UseCaseGetCategory.GetCategoryInput(exampleCategory.Id);
        var useCase = new UseCaseGetCategory.GetCategory(repositoryMock.Object);

        // Act
        UseCaseGetCategory.GetCategoryOutput output = await useCase.Handle(input, CancellationToken.None);

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
}
