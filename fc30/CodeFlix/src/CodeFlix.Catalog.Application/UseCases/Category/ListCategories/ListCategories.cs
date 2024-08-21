using CodeFlix.Catalog.Application.UseCases.Category.Common;
using CodeFlix.Catalog.Domain.Repository;
using CodeFlix.Catalog.Domain.SeedWork.SearchableRepository;

namespace CodeFlix.Catalog.Application.UseCases.Category.ListCategories;

public class ListCategories : IListCategories
{
    private readonly ICategoryRepository _categoryRepository;

    public ListCategories(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<ListCategoriesOutput> Handle(ListCategoriesInput request, CancellationToken cancellationToken)
    {
        var searchOutput = await _categoryRepository.SearchAsync(new SearchInput(
            request.Page,
            request.PerPage,
            request.Search,
            request.Sort,
            request.Dir
        ), cancellationToken);

        return new ListCategoriesOutput(
            searchOutput.CurrentPage,
            searchOutput.PerPage, 
            searchOutput.Total, 
            searchOutput.Items.Select(i => CategoryModelOutput.FromCategory(i)).ToList()
        );
    }
}
