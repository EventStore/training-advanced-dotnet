using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Scheduling.Infrastructure.Headers;

public class CommandMetadataOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Parameters ??= new List<OpenApiParameter>();

        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "X-CorrelationId",
            In= ParameterLocation.Header,
            Description = "Correlation Id",
            Required = true
        });
        
        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "X-CausationId",
            In= ParameterLocation.Header,
            Description = "Causation Id",
            Required = true
        });
    }
}