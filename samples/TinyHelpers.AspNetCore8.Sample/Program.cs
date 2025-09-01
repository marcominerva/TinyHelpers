using Microsoft.OpenApi.Models;
using TinyHelpers.AspNetCore.Extensions;
using TinyHelpers.AspNetCore.Swagger;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRequestLocalization("it", "en", "de");

builder.Services.AddEndpointsApiExplorer();

// Add Swagger parameters that are required for all endpoints.
builder.Services.AddSwaggerOperationParameters(options =>
{
    options.Parameters.Add(new()
    {
        Name = "x-tenant",
        In = ParameterLocation.Header,
        Required = true,
        Schema = OpenApiSchemaHelper.CreateStringSchema()
    });

    options.Parameters.Add(new()
    {
        Name = "x-environment",
        In = ParameterLocation.Header,
        Schema = OpenApiSchemaHelper.CreateSchema<Environment>(Environment.Production)
    });

    options.Parameters.Add(new()
    {
        Name = "code",
        In = ParameterLocation.Query,
        Schema = OpenApiSchemaHelper.CreateSchema<Guid>("string", "uuid")
    });

    options.Parameters.Add(new()
    {
        Name = "version",
        In = ParameterLocation.Query,
        Schema = OpenApiSchemaHelper.CreateSchema<int>("integer", "int32", 1)
    });
});

builder.Services.AddSwaggerGen(options =>
{
    // Add Accept-Language header to all endpoints.
    options.AddAcceptLanguageHeader();

    // Add a default (error) response to all endpoints.
    options.AddDefaultProblemDetailsResponse();

    // Enable Swagger integration for custom parameters.
    options.AddOperationParameters();
});

// Add default problem details and exception handler.
builder.Services.AddDefaultProblemDetails();
builder.Services.AddDefaultExceptionHandler();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.UseExceptionHandler();
app.UseStatusCodePages();

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();
app.UseRequestLocalization();

app.MapGet("/api/sample", () =>
{
    var language = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
    return TypedResults.NoContent();
})
.WithOpenApi();

app.MapPost("/api/exception", () =>
{
    throw new Exception("This is an exception", innerException: new HttpRequestException("This is an inner exception"));
})
.ProducesProblem(StatusCodes.Status400BadRequest)
.WithOpenApi();

app.Run();

public enum Environment
{
    Development,
    Staging,
    Production
}