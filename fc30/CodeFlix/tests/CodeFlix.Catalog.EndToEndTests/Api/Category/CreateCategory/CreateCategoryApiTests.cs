using System.Net;
using CodeFlix.Catalog.Application.UseCases.Category.Common;
using CodeFlix.Catalog.Application.UseCases.Category.CreateCategory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    [Trait("EndToEnd/Api", "Category/Create Endpoints")]
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
        response!.StatusCode.Should().Be(HttpStatusCode.Created);

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

    [Theory(DisplayName = nameof(ThrowWhenCanNotInstantiateAggregate))]
    [Trait("EndToEnd/Api", "Category/Create Endpoints")]
    [MemberData(nameof(CreateCategoryApiTestsDataGenerator.GetInvalidInputs), MemberType = typeof(CreateCategoryApiTestsDataGenerator))]
    public async Task ThrowWhenCanNotInstantiateAggregate(CreateCategoryInput input, string expectedErrorMessage)
    {
        // Act
        var (response, output) = await _fixture.ApiClient.Post<ProblemDetails>(
            "/api/categories",
            input
            );

        // Assert
        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);

        output.Should().NotBeNull();
        output!.Title.Should().Be("One or more validation errors ocurred");
        output.Detail.Should().Be(expectedErrorMessage);
        output.Type.Should().Be("UnprocessableEntity");
        output.Status.Should().Be(StatusCodes.Status422UnprocessableEntity);
    }
}
