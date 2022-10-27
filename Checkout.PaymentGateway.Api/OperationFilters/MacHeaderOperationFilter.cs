using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Checkout.PaymentGateway.Api.OperationFilters
{
    /// <summary>Used to represent the mac header within the Swagger documentation.</summary>
    public class MacHeaderOperationFilter : IOperationFilter
    {
        /// <summary>Applies the mac header operation filter to the OpenApiOperation instance.</summary>
        /// <param name="operation">The OpenApiOperation instance.</param>
        /// <param name="context">The OperationFilterContext instance.</param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
            {
                operation.Parameters = new List<OpenApiParameter>();
            }

            operation.Parameters.Add(
                new OpenApiParameter()
                {
                    Name = "mac",
                    In = ParameterLocation.Header,
                    Required = false,
                    Description =
                            "A hash value using the SHA256 algorithm of the request body, UTC timestamp and secret, for example"
                            + " data being {\"Test\": true}2019110812secret the hash would be 9d5d046f04ea07ab53727c6cfe40c18ad9cddd20d1cd1b5ef240bb22f7b72946,"
                            + " please note if HTTP method used is GET or there is no body create the hash with an empty body.",
                    Schema = new OpenApiSchema { Type = "string" },
                });
        }
    }
}
