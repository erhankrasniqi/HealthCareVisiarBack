using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CQRS_Decorator.API.Filters
{
    public class BookAppointmentRequestSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type.Name == "BookAppointmentRequest")
            {
                schema.Example = new OpenApiObject
                {
                    ["doctorId"] = new OpenApiString("3fa3bf64-5717-4562-b3fc-2c963f66afa6"),
                    ["appointmentDate"] = new OpenApiString("2024-12-20"),
                    ["startTime"] = new OpenApiString("09:00:00"),
                    ["endTime"] = new OpenApiString("09:30:00"),
                    ["reason"] = new OpenApiString("Annual checkup")
                };
            }
        }
    }
}
