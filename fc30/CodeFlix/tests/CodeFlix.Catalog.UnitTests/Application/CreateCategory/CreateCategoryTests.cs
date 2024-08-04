using CodeFlix.Catalog.Application.UseCases.Category.CreateCategory;
using CodeFlix.Catalog.Domain.Entity;
using CodeFlix.Catalog.Domain.Exceptions;
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

    [Theory(DisplayName = nameof(Success))]
    [Trait("Application", "CreateCategory - Use Cases")]
    [MemberData(nameof(CreateCategoryTestsGenerator.GetValidInputs), MemberType = typeof(CreateCategoryTestsGenerator))]
    public async Task Success(CreateCategoryInput input)
    {
        // Arrange
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUowMock();
        var useCase = new UseCases.CreateCategory(repositoryMock.Object, unitOfWorkMock.Object);
        // CreateCategoryInput input = _fixture.GetValidInput();

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

    [Theory(DisplayName = nameof(ThrowWhenCanNotInstantiateCategory))]
    [Trait("Application", "CreateCategory - Use Cases")]
    [MemberData(nameof(CreateCategoryTestsGenerator.GetInvalidInputs), parameters: 24, MemberType = typeof(CreateCategoryTestsGenerator))]
    public async Task ThrowWhenCanNotInstantiateCategory(CreateCategoryInput input, string exceptionMessage)
    {
        // Arrange
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUowMock();
        var useCase = new UseCases.CreateCategory(repositoryMock.Object, unitOfWorkMock.Object);

        // Act
        Func<Task> task = async () => await useCase.HandleAsync(input, CancellationToken.None);

        // Assert
        await task.Should().ThrowAsync<EntityValidationException>().WithMessage(exceptionMessage);
    }
}
