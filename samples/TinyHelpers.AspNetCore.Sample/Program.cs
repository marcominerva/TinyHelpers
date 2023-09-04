using Microsoft.OpenApi.Models;
using MinimalHelpers.OpenApi;
using TinyHelpers.AspNetCore.Extensions;
using TinyHelpers.AspNetCore.Swagger;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRequestLocalization("it", "en", "de");

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Add parameters that are required for all operations.
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
    options.AddMissingSchemas();

    options.AddAcceptLanguageHeader();

    // Enable Swagger integration for custom parameters.
    options.AddOperationParameters();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/api/sample", () =>
{
    return TypedResults.NoContent();
})
.WithOpenApi();

app.Run();

public enum Environment
{
    Development,
    Staging,
    Production
}