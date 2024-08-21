using CodeFlix.Catalog.Application.UseCases.Category.ListCategories;
using CodeFlix.Catalog.Domain.Entity;
using CodeFlix.Catalog.Domain.SeedWork.SearchableRepository;
using UseCaseListCategories = CodeFlix.Catalog.Application.UseCases.Category.ListCategories;

namespace CodeFlix.Catalog.UnitTests.Application.ListCategories;

[Collection(nameof(ListCategoriesTestsFixture))]
public class ListCategoriesTests
{
    private readonly ListCategoriesTestsFixture _fixture;

    public ListCategoriesTests(ListCategoriesTestsFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(ListSuccess))]
    [Trait("Application", "ListCategories - Use Cases")]
    public async Task ListSuccess() {
        var repositoryMock = _fixture.GetRepositoryMock();
        var input = new ListCategoriesInput(
            page: 2,
            perPage: 15,
            search: "shirt",
            sort: "name",
            dir: SearchOrder.Asc
        );
        var categories = _fixture.GetCategories();
        var repositorySearchOutput = new SearchOutput<Category>(
            currentPage: input.Page,
            perPage: input.PerPage,
            items: categories,
            total: categories.Count
        );
        repositoryMock.Setup(x => x.SearchAsync(
            It.Is<SearchInput>(i => 
                i.Page == input.Page
                && i.PerPage == input.PerPage
                && i.Search == input.Search
                && i.OrderBy == input.Sort
                && i.Order == input.Dir
            ),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(repositorySearchOutput);
        var useCase = new UseCaseListCategories.ListCategories(repositoryMock.Object);

        // Act
        var output = await useCase.Handle(input, CancellationToken.None);

        // Assert
        output.Should().NotBeNull();
        output.Page.Should().Be(repositorySearchOutput.CurrentPage);
        output.PerPage.Should().Be(repositorySearchOutput.PerPage);
        output.Total.Should().Be(repositorySearchOutput.Total);
        output.Items.Should().HaveCount(repositorySearchOutput.Items.Count);
        output.Items.ToList().ForEach(outputItem => {
            var repositoryCategory = repositorySearchOutput.Items.FirstOrDefault(x => x.Id == outputItem.Id);
            outputItem.Should().NotBeNull();
            outputItem.Name.Should().Be(repositoryCategory!.Name);
            outputItem.Description.Should().Be(repositoryCategory.Description);
            outputItem.IsActive.Should().Be(repositoryCategory.IsActive);
            outputItem.CreatedAt.Should().Be(repositoryCategory.CreatedAt);
        });
        repositoryMock.Verify(x => x.SearchAsync(
            It.Is<SearchInput>(p => 
                p.Page == input.Page
                && p.PerPage == input.PerPage
                && p.Search == input.Search
                && p.OrderBy == input.Sort
                && p.Order == input.Dir),
            It.IsAny<CancellationToken>()
            ), Times.Once
        );
        repositoryMock.VerifyNoOtherCalls();
    }
}
