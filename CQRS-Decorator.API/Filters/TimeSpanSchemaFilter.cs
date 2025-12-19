using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CQRS_Decorator.API.Filters
{
    public class TimeSpanSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type == typeof(TimeSpan) || context.Type == typeof(TimeSpan?))
            {
                schema.Type = "string";
                schema.Format = "time-span";
                schema.Example = new OpenApiString("09:00:00");
                schema.Pattern = @"^([01]?[0-9]|2[0-3]):[0-5][0-9]:[0-5][0-9]$";
            }
        }
    }
}
