using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Checkout.PaymentGateway.Api.Helpers
{
    /// <summary>Used to implement extensions for the ResourceExecutingContext class.</summary>
    public static class ResourceExecutingContextExtensions
    {
        /// <summary>Used to assign a bad request object result to the result property of ResourceExecutingContext.</summary>
        /// <param name="resourceExecutingContext">The resource executing context.</param>
        /// <param name="model">The model to be used in the result.</param>
        public static void AssignResultBadRequest(this ResourceExecutingContext resourceExecutingContext, object model)
        {
            if (resourceExecutingContext == null)
            {
                return;
            }

            resourceExecutingContext.Result = new BadRequestObjectResult(model);
        }
    }
}
