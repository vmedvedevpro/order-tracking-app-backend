using Microsoft.AspNetCore.HttpLogging;

using OrderTrackingApp.Backend.Infrastructure;
using OrderTrackingApp.Backend.Infrastructure.Persistence;
using OrderTrackingApp.Backend.WebApi;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddInfrastructure(builder.Configuration, builder.Host);

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

app.Run();
