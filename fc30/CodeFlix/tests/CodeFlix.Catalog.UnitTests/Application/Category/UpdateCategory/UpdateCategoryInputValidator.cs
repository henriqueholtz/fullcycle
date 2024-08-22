using CodeFlix.Catalog.Application.UseCases.Category.UpdateCategory;
using FluentValidation;

namespace CodeFlix.Catalog.UnitTests.Application.Category.UpdateCategory;

public class UpdateCategoryInputValidator : AbstractValidator<UpdateCategoryInput>
{
    public UpdateCategoryInputValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
