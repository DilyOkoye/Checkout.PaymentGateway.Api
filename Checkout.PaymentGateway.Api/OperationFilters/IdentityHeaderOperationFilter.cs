using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Checkout.PaymentGateway.Api.OperationFilters
{
    /// <summary>Used to represent the identity header within the Swagger documentation.</summary>
    public class IdentityHeaderOperationFilter : IOperationFilter
    {
        /// <summary>Applies the identity header operation filter to the OpenApiOperation instance.</summary>
        /// <param name="operation">The OpenApiOperation instance.</param>
        /// <param name="context">The OperationFilterContext instance.</param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
            {
                operation.Parameters = new List<OpenApiParameter>();
            }

            operation.Parameters.Add(
                new OpenApiParameter
                {
                    Name = "identity",
                    In = ParameterLocation.Header,
                    Required = false,
                    Description = "An identity used with the mac to identify which secret the Checkout API will use when calculating the hash.",
                    Schema = new OpenApiSchema { Type = "string" },
                });
        }
    }
}
