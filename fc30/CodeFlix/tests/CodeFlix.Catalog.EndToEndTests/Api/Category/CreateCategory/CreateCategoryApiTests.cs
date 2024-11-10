using CodeFlix.Catalog.Application.UseCases.Category.Common;
using DomainEntity = CodeFlix.Catalog.Domain.Entity;

namespace CodeFlix.Catalog.EndToEndTests.Api.Category.CreateCategory;

[Collection(nameof(CreateCategoryApiTestsFixture))]
public class CreateCategoryApiTests
{
    private readonly CreateCategoryApiTestsFixture _fixture;

    public CreateCategoryApiTests(CreateCategoryApiTestsFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(CreateCategory))]
    [Trait("EndToEnd/Api", "Category Endpoints")]
    public async Task CreateCategory()
    {
        // Arrange
        var input = _fixture.GetValidInput();

        // Act
        var (response, output) = await _fixture.ApiClient.Post<CategoryModelOutput>(
            "/api/categories",
            input
            );

        // Assert
        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        output.Should().NotBeNull();
        output!.Id.Should().NotBeEmpty();
        output.Name.Should().Be(input.Name);
        output.IsActive.Should().Be(input.IsActive);
        output.Description.Should().Be(input.Description);
        output.CreatedAt.Should().NotBeSameDateAs(default);

        DomainEntity.Category? dbCategory = await _fixture.Persistence.GetByIdAsync(output.Id);
        dbCategory.Should().NotBeNull();
        dbCategory!.Id.Should().NotBeEmpty();
        dbCategory.Name.Should().Be(input.Name);
        dbCategory.IsActive.Should().Be(input.IsActive);
        dbCategory.Description.Should().Be(input.Description);
        dbCategory.CreatedAt.Should().NotBeSameDateAs(default);
    }
}
