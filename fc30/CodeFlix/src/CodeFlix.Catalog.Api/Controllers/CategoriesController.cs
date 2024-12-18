using CodeFlix.Catalog.Application.UseCases.Category.Common;
using CodeFlix.Catalog.Application.UseCases.Category.CreateCategory;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CodeFlix.Catalog.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(typeof(CategoryModelOutput), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Create([FromBody] CreateCategoryInput input, CancellationToken cancellationToken)
        {
            CategoryModelOutput output = await _mediator.Send(input, cancellationToken);
            return CreatedAtAction(nameof(Create), new { output.Id }, output);
        }
    }
}
