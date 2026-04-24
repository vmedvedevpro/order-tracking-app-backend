using Microsoft.AspNetCore.Diagnostics;

namespace OrderTrackingApp.Backend.WebApi.ExceptionHandlers;

public class BadRequestExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not BadHttpRequestException badHttpRequestException) return false;

        await Results.BadRequest(badHttpRequestException).ExecuteAsync(httpContext);

        return true;
    }
}
