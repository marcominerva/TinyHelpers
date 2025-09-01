using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using TinyHelpers.AspNetCore.Extensions;
using TinyHelpers.AspNetCore.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddRequestLocalization("it", "en", "de");

// Add OpenAPI parameters that are required for all endpoints.
builder.Services.AddOpenApiOperationParameters(options =>
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
        Name = "Version",
        In = ParameterLocation.Query,
        Schema = OpenApiSchemaHelper.CreateSchema<int>("integer", "int32", 1)
    });
});

builder.Services.AddOpenApi(options =>
{
    // Add Accept-Language header to all endpoints.
    options.AddAcceptLanguageHeader();

    // Add a default (error) response to all endpoints.
    options.AddDefaultProblemDetailsResponse();

    // Enable OpenAPI integration for custom parameters.
    options.AddOperationParameters();

    // Remove Servers list in OpenAPI (only needed in Development).
    options.RemoveServerList();

    // Fix the ignored JsonNumberHandling attribute.
    options.WriteNumberAsString();

    // Describe all query string parameters in Camel Case.
    options.DescribeAllParametersInCamelCase();

    // Correctly define enum serialization.
    options.EnableEnumSupport();

    // Add time examples for TimeSpan and TimeOnly fields.
    options.AddTimeExamples();
});

// Add default problem details and exception handler.
builder.Services.AddDefaultProblemDetails();
builder.Services.AddDefaultExceptionHandler();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.UseExceptionHandler();
app.UseStatusCodePages();

app.MapOpenApi();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", builder.Environment.ApplicationName);
});

app.UseRouting();
app.UseRequestLocalization();

app.MapGet("/api/sample", () =>
{
    var language = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
    return TypedResults.NoContent();
});

app.MapGet("/api/json-number-as-string", () =>
{
    return TypedResults.Ok(new RandomNumber());
});

app.MapPost("/api/exception", () =>
{
    throw new Exception("This is an exception", innerException: new HttpRequestException("This is an inner exception"));
})
.ProducesProblem(StatusCodes.Status500InternalServerError);

app.MapPost("/api/time", (TimeInput input) =>
{
    return TypedResults.Ok(input);
});

app.MapGet("/api/status", () => new StatusResult { Status = Status.Closed });

app.Run();

public enum Environment
{
    Development,
    Staging,
    Production
}

internal record RandomNumber()
{
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    public int Number { get; init; } = Random.Shared.Next(0, 100);
};

public record class TimeInput(TimeSpan? TimeSpan, TimeOnly? TimeOnly);

public class StatusResult
{
    public Status Status { get; set; }

    public Status? NullableStatus { get; set; }
}

public enum Status
{
    Open,
    Closed,
    [JsonStringEnumMemberName("Cancelled")]
    Removed
}