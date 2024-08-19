using CodeFlix.Catalog.Application.UseCases.Category.UpdateCategory;
using CodeFlix.Catalog.Domain.Entity;
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

    [Theory(DisplayName = nameof(Success))]
    [Trait("Application", "UpdateCategory - Use Cases")]
    [MemberData(nameof(UpdateCategoryTestsGenerator.GetCategoriesToUpdate), parameters: 10, MemberType = typeof(UpdateCategoryTestsGenerator))]
    public async Task Success(Category category, UpdateCategoryInput input) {
        // Arrange
        var repositoryMock = _fixture.GetRepositoryMock();
        var uowMock = _fixture.GetUowMock();
        repositoryMock.Setup(r => r.GetAsync(category.Id, It.IsAny<CancellationToken>())).ReturnsAsync(category);

        UseCaseUpdateCategory.UpdateCategory useCase = new(repositoryMock.Object, uowMock.Object);

        // Act
        var output = await useCase.Handle(input, CancellationToken.None);

        // Assert
        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(input.IsActive);

        repositoryMock.Verify(r => r.GetAsync(category.Id, It.IsAny<CancellationToken>()), Times.Once());
        repositoryMock.Verify(r => r.UpdateAsync(category, It.IsAny<CancellationToken>()), Times.Once());
        repositoryMock.VerifyNoOtherCalls();

        uowMock.Verify(r => r.CommitAsync(It.IsAny<CancellationToken>()), Times.Once());
        uowMock.VerifyNoOtherCalls();
    }

}
