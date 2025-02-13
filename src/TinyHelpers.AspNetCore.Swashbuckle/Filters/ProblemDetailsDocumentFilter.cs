using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TinyHelpers.AspNetCore.Swagger.Filters;

internal class ProblemDetailsDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        => context.SchemaGenerator.GenerateSchema(typeof(ProblemDetails), context.SchemaRepository);
}