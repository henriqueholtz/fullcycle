using CodeFlix.Catalog.Application.Exceptions;
using CodeFlix.Catalog.Application.UseCases.Category.UpdateCategory;
using CodeFlix.Catalog.Domain.Entity;
using CodeFlix.Catalog.Domain.Exceptions;
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

    [Theory(DisplayName = nameof(FullUpdateShouldBeSuccessful))]
    [Trait("Application", "UpdateCategory - Use Cases")]
    [MemberData(nameof(UpdateCategoryTestsGenerator.GetCategoriesToUpdate), parameters: 10, MemberType = typeof(UpdateCategoryTestsGenerator))]
    public async Task FullUpdateShouldBeSuccessful(Category category, UpdateCategoryInput input) {
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
        output.IsActive.Should().Be((bool)input.IsActive!);

        repositoryMock.Verify(r => r.GetAsync(category.Id, It.IsAny<CancellationToken>()), Times.Once());
        repositoryMock.Verify(r => r.UpdateAsync(category, It.IsAny<CancellationToken>()), Times.Once());
        repositoryMock.VerifyNoOtherCalls();

        uowMock.Verify(r => r.CommitAsync(It.IsAny<CancellationToken>()), Times.Once());
        uowMock.VerifyNoOtherCalls();
    }

    [Theory(DisplayName = nameof(UpdateWithoutIsActiveShouldBeSuccessful))]
    [Trait("Application", "UpdateCategory - Use Cases")]
    [MemberData(nameof(UpdateCategoryTestsGenerator.GetCategoriesToUpdate), parameters: 10, MemberType = typeof(UpdateCategoryTestsGenerator))]
    public async Task UpdateWithoutIsActiveShouldBeSuccessful(Category category, UpdateCategoryInput baseInput) {
        // Arrange
        var input = new UpdateCategoryInput(baseInput.Id, baseInput.Name, baseInput.Description);
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
        output.IsActive.Should().Be(category.IsActive);

        repositoryMock.Verify(r => r.GetAsync(category.Id, It.IsAny<CancellationToken>()), Times.Once());
        repositoryMock.Verify(r => r.UpdateAsync(category, It.IsAny<CancellationToken>()), Times.Once());
        repositoryMock.VerifyNoOtherCalls();

        uowMock.Verify(r => r.CommitAsync(It.IsAny<CancellationToken>()), Times.Once());
        uowMock.VerifyNoOtherCalls();
    }

    [Theory(DisplayName = nameof(UpdateOnlyNameShouldBeSuccessful))]
    [Trait("Application", "UpdateCategory - Use Cases")]
    [MemberData(nameof(UpdateCategoryTestsGenerator.GetCategoriesToUpdate), parameters: 10, MemberType = typeof(UpdateCategoryTestsGenerator))]
    public async Task UpdateOnlyNameShouldBeSuccessful(Category category, UpdateCategoryInput baseInput) {
        // Arrange
        var input = new UpdateCategoryInput(baseInput.Id, baseInput.Name);
        var repositoryMock = _fixture.GetRepositoryMock();
        var uowMock = _fixture.GetUowMock();
        repositoryMock.Setup(r => r.GetAsync(category.Id, It.IsAny<CancellationToken>())).ReturnsAsync(category);

        UseCaseUpdateCategory.UpdateCategory useCase = new(repositoryMock.Object, uowMock.Object);

        // Act
        var output = await useCase.Handle(input, CancellationToken.None);

        // Assert
        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(category.Description);
        output.IsActive.Should().Be(category.IsActive);

        repositoryMock.Verify(r => r.GetAsync(category.Id, It.IsAny<CancellationToken>()), Times.Once());
        repositoryMock.Verify(r => r.UpdateAsync(category, It.IsAny<CancellationToken>()), Times.Once());
        repositoryMock.VerifyNoOtherCalls();

        uowMock.Verify(r => r.CommitAsync(It.IsAny<CancellationToken>()), Times.Once());
        uowMock.VerifyNoOtherCalls();
    }

    [Fact(DisplayName = nameof(ThrowNotFoundExceptionWhenCategoryIsNotFound))]
    [Trait("Application", "UpdateCategory - Use Cases")]
    public async Task ThrowNotFoundExceptionWhenCategoryIsNotFound () {
        // Arrange
        var repositoryMock = _fixture.GetRepositoryMock();
        var uowMock = _fixture.GetUowMock();
        var input = _fixture.GetValidInput(Guid.NewGuid());
        repositoryMock.Setup(r => r.GetAsync(input.Id, It.IsAny<CancellationToken>())).ThrowsAsync(new NotFoundException($"Category {input.Id} not found."));
        UseCaseUpdateCategory.UpdateCategory useCase = new(repositoryMock.Object, uowMock.Object);

        // Act
        var task = async () => await useCase.Handle(input, CancellationToken.None);

        // Assert
        await task.Should().ThrowAsync<NotFoundException>();
        repositoryMock.Verify(r => r.GetAsync(input.Id, It.IsAny<CancellationToken>()), Times.Once());
        repositoryMock.VerifyNoOtherCalls();
        uowMock.VerifyNoOtherCalls();
    }

    [Theory(DisplayName = nameof(ThrowEntityValidationExceptionWhenCanNotUpdateCategory))]
    [Trait("Application", "UpdateCategory - Use Cases")]
    [MemberData(nameof(UpdateCategoryTestsGenerator.GetInvalidInputs), parameters: 12, MemberType = typeof(UpdateCategoryTestsGenerator))]
    public async Task ThrowEntityValidationExceptionWhenCanNotUpdateCategory(UpdateCategoryInput input, string expectedExceptionMessage) {
        // Arrange
        var repositoryMock = _fixture.GetRepositoryMock();
        var uowMock = _fixture.GetUowMock();
        var category = _fixture.GetValidCategory();
        input.Id = category.Id;
        repositoryMock.Setup(r => r.GetAsync(category.Id, It.IsAny<CancellationToken>())).ReturnsAsync(category);
        UseCaseUpdateCategory.UpdateCategory useCase = new(repositoryMock.Object, uowMock.Object);

        // Act
        var task = async () => await useCase.Handle(input, CancellationToken.None);

        // Assert
        await task.Should().ThrowAsync<EntityValidationException>().WithMessage(expectedExceptionMessage);
        repositoryMock.Verify(r => r.GetAsync(category.Id, It.IsAny<CancellationToken>()), Times.Once());
        repositoryMock.VerifyNoOtherCalls();
        uowMock.VerifyNoOtherCalls();
    }
}
