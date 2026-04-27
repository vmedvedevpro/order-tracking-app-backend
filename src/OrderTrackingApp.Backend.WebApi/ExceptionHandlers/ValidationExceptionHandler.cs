using FluentValidation;

using Microsoft.AspNetCore.Diagnostics;

namespace OrderTrackingApp.Backend.WebApi.ExceptionHandlers;

public class ValidationExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not ValidationException validationException) return false;

        await Results.ValidationProblem(
                         validationException.Errors
                                            .GroupBy(x => x.PropertyName).ToDictionary(
                                                g => g.Key,
                                                g => g.Select(x => x.ErrorMessage).ToArray()
                                                ))
                     .ExecuteAsync(httpContext);

        return true;
    }
}
