using MediatR;

namespace CodeFlix.Catalog.Application.UseCases.Category.DeleteCategory;

public class DeleteCategoryInput : IRequest
{
    public Guid Id { get; set; }
    public DeleteCategoryInput(Guid id)
    {
        Id = id;
    }
}
