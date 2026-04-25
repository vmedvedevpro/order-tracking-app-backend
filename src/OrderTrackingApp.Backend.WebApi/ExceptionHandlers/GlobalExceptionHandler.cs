using Microsoft.AspNetCore.Diagnostics;

namespace OrderTrackingApp.Backend.WebApi.ExceptionHandlers;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        await Results.Problem(
                         exception.Message,
                         statusCode: 500,
                         extensions: exception.InnerException is not null
                                         ? new Dictionary<string, object?> { { "inner-exception", exception.InnerException.Message } }
                                         : null)
                     .ExecuteAsync(httpContext);

        return true;
    }
}
