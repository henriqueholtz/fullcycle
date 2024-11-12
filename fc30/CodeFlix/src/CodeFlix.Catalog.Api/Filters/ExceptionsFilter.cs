using System.Net;
using CodeFlix.Catalog.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CodeFlix.Catalog.Api.Filters;

public class ExceptionsFilter : IExceptionFilter
{
    private readonly IHostEnvironment _env;

    public ExceptionsFilter(IHostEnvironment env)
    {
        _env = env;
    }

    public void OnException(ExceptionContext context)
    {
        var details = new ProblemDetails();
        var ex = context.Exception;

        if (_env.IsDevelopment())
            details.Extensions.Add("StackTrace", ex.StackTrace);

        if (ex is EntityValidationException)
        {
            details.Title = "One or more validation errors ocurred";
            details.Status = StatusCodes.Status422UnprocessableEntity;
            details.Type = "UnprocessableEntity";
            details.Detail = (ex as EntityValidationException)!.Message;
        }
        else
        {
            details.Title = "An unexpected error ocurred";
            details.Status = StatusCodes.Status500InternalServerError;
            details.Type = "UnexpectedError";
            details.Detail = ex.Message;
        }

        context.HttpContext.Response.StatusCode = (int)details.Status;
        context.Result = new ObjectResult(details);
        context.ExceptionHandled = true;
    }
}
