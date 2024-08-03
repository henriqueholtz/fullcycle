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
    [MemberData(nameof(GetValidInputs))]
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

    public static IEnumerable<object[]> GetValidInputs()
    {
        CreateCategoryTestsFixture fixture = new();
        return new List<object[]>() {
            new [] { fixture.GetValidInput() },
            new [] { new CreateCategoryInput(fixture.GetValidCategoryName()) },
            new [] { new CreateCategoryInput(fixture.GetValidCategoryName(), fixture.GetValidCategoryDescription()) }
        };
    }

    [Theory(DisplayName = nameof(ThrowWhenCanNotInstantiateAggregate))]
    [Trait("Application", "CreateCategory - Use Cases")]
    [MemberData(nameof(GetInvalidInputs))]
    public async Task ThrowWhenCanNotInstantiateAggregate(CreateCategoryInput input, string exceptionMessage)
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

    public static IEnumerable<object[]> GetInvalidInputs() 
    {
        var fixture = new CreateCategoryTestsFixture();
        List<object[]> invalidInputs = new();

        // Name too short
        var invalidInput = fixture.GetValidInput();
        invalidInput.Name = invalidInput.Name.Substring(0, 2);
        invalidInputs.Add(new object[] { invalidInput, "Name should be at least 3 characters long" });
        
        // Name too long
        invalidInput = fixture.GetValidInput();
        while (invalidInput.Name.Length <= 255)
            invalidInput.Name += $" {fixture.GetValidCategoryName()}";
        invalidInputs.Add(new object[] { invalidInput, "Name should be less or equal 255 characters long" });
        
        // Null descrition
        invalidInput = fixture.GetValidInput();
        invalidInput.Description = null;
        invalidInputs.Add(new object[] { invalidInput, "Description should not be null" });
        
        // Description too long
        invalidInput = fixture.GetValidInput();
        while (invalidInput.Description.Length <= 10_000)
            invalidInput.Description += $" {fixture.GetValidCategoryDescription()}";
        invalidInputs.Add(new object[] { invalidInput, "Description should be less or equal 10000 characters long" });

        return invalidInputs;
    }
}
