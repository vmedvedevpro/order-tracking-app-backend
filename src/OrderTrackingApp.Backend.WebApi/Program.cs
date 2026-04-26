using Microsoft.AspNetCore.HttpLogging;

using OrderTrackingApp.Backend.Application;
using OrderTrackingApp.Backend.Infrastructure;
using OrderTrackingApp.Backend.Infrastructure.Configuration;
using OrderTrackingApp.Backend.Infrastructure.Persistence;
using OrderTrackingApp.Backend.WebApi;
using OrderTrackingApp.Backend.WebApi.Endpoints.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
       .AddEnvironmentVariables()
       .AddUserSecrets<Program>(true);

builder.Services
       .AddEndpointsApiExplorer()
       .AddSwaggerGen()
       .AddProblemDetails()
       .ConfigureJsonOptions()
       .AddExceptionHandlers()
       .AddHttpLogging(options =>
                       {
                           options.LoggingFields = HttpLoggingFields.All;
                           options.CombineLogs = true;
                       });

builder.Services.AddApplication()
       .AddInfrastructure(builder.Configuration, builder.Host);

var app = builder.Build();

await app.InitialiseDatabaseAsync();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger()
       .UseSwaggerUI();
}

app.UseExceptionHandler()
   .UseHttpLogging();

app.UseHttpsRedirection();

app.UseTelemetry();

app.MapEndpoints();

app.Run();
