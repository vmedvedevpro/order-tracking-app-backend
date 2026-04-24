using Microsoft.AspNetCore.Diagnostics;

using OrderTrackingApp.Backend.Application.Common.Exceptions;

namespace OrderTrackingApp.Backend.WebApi.ExceptionHandlers;

public class AppExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not AppException appException) return false;

        await Results.Problem(
                         appException.Message,
                         statusCode: 500,
                         extensions: exception.InnerException is not null
                                         ? new Dictionary<string, object?> { { "inner-exception", exception.InnerException.Message } }
                                         : null)
                     .ExecuteAsync(httpContext);

        return true;
    }
}
