using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

using Microsoft.AspNetCore.Mvc;

using OrderTrackingApp.Backend.WebApi.ExceptionHandlers;

namespace OrderTrackingApp.Backend.WebApi;

internal static class DependencyInjection
{
    internal static IServiceCollection ConfigureJsonOptions(this IServiceCollection services) =>
        services.ConfigureHttpJsonOptions(o =>
                                          {
                                              o.SerializerOptions.Encoder = JavaScriptEncoder.Create(
                                                  UnicodeRanges.BasicLatin,
                                                  UnicodeRanges.Cyrillic);
                                              o.SerializerOptions.Converters.Add(new JsonStringEnumConverter(allowIntegerValues: true));
                                              o.SerializerOptions.PropertyNameCaseInsensitive = true;
                                          })
                .Configure<JsonOptions>(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
                .AddSingleton<JsonSerializerOptions>(o =>
                                                     {
                                                         var options = new JsonSerializerOptions(JsonSerializerOptions.Web);
                                                         options.Converters.Add(new JsonStringEnumConverter(allowIntegerValues: true));
                                                         return options;
                                                     });

    internal static IServiceCollection AddExceptionHandlers(this IServiceCollection services) =>
        services.AddExceptionHandler<BadRequestExceptionHandler>()
                .AddExceptionHandler<ValidationExceptionHandler>()
                .AddExceptionHandler<NotFoundExceptionHandler>()
                .AddExceptionHandler<GlobalExceptionHandler>();
}
