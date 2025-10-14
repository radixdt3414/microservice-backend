using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace buildingBlock.Exceptions
{
    public class CustomExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, System.Exception exception, CancellationToken cancellationToken)
        {

            (int statusCode, string detils, string title) = exception switch
            {
                BadRequestException => (
                    (int)StatusCodes.Status400BadRequest,
                    exception.InnerException?.Message ?? string.Empty,
                    exception.Message
                    ),
                NotFoundException => (
                    (int)StatusCodes.Status404NotFound,
                    exception.InnerException?.Message ?? string.Empty,
                    exception.Message
                    ),
                ValidationException => (
                    (int)StatusCodes.Status400BadRequest,
                    exception.InnerException?.Message ?? string.Empty,
                    exception.Message
                    ),
                _ => ((int)StatusCodes.Status500InternalServerError,
                    exception.InnerException?.Message ?? string.Empty,
                    exception.Message)
            };

            ProblemDetails errorObj = new ProblemDetails()
            {
                Status = statusCode,
                Detail = detils,
                Title = title
            };
            
            if(exception.InnerException is ValidationException validationException)
            {
                errorObj.Extensions.Add("BadRequest", validationException);
            }
            httpContext.Response.ContentType = "application/problem+json";
            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsJsonAsync(errorObj);
            return true;
        }
    }
}