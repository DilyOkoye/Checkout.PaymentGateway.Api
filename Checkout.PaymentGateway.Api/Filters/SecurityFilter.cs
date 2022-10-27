using Checkout.PaymentGateway.Api.Configurations;
using Checkout.PaymentGateway.Api.Helpers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using System.Text;

namespace Checkout.PaymentGateway.Api.Filters
{
    /// <summary>A custom filter used to validate the incoming body with the identity header and mac header sent in via the merchant.</summary>
    public class SecurityFilter : Attribute, IAsyncResourceFilter
    {
        /// <summary>The validation error message returned to the user when the security validation fails.</summary>
        private const string ValidationErrorMessage =
            "The header MAC has been validated to be incorrect with the given body and identity";

        /// <summary>A method to apply the security validation on the request before reaching the requested action.</summary>
        /// <param name="context">The ResourceExecutingContext instance.</param>
        /// <param name="next">The ResourceExecutionDelegate which is called to continue alone the pipeline.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

            //get SecurityConfiguration, can be done via Dependency Injection
            var securityConfiguration = configuration.GetSection("SecurityConfiguration")
             .Get<SecurityConfiguration>();

            if (securityConfiguration.IsEnabled)
            {
                StringValues identityValue;
                StringValues macValue;

                var identityResult = context.HttpContext.Request.Headers.TryGetValue("identity", out identityValue);
                var macResult = context.HttpContext.Request.Headers.TryGetValue("mac", out macValue);

                if (identityResult && macResult)
                {
                    var body = await RetrieveBodyAsync(context.HttpContext.Request);

                    if (!ValidateData(identityValue, body, macValue, securityConfiguration))
                    {
                        // Short circuit the request and return an error response.
                        context.AssignResultBadRequest(new { ErrorMessage = ValidationErrorMessage });
                        return;
                    }
                }
                else
                {
                    // Short circuit the request and return an error response.
                    context.AssignResultBadRequest(new { ErrorMessage = ValidationErrorMessage });
                    return;
                }
            }

            // Call to proceed along the rest of the request pipeline.
            await next();

            //// Code here would execute after action execution.
        }

        /// <summary>Used to retrieve the body of the specified HttpRequest.</summary>
        /// <param name="httpRequest">The current http request.</param>
        /// <returns>The <see cref="Task"/> request body.</returns>
        private static async Task<string> RetrieveBodyAsync(HttpRequest httpRequest)
        {
            string body;

            httpRequest.EnableBuffering();

            using (var streamReader = new StreamReader(httpRequest.Body, Encoding.UTF8, true, 1024, true))
            {
                body = await streamReader.ReadToEndAsync();
                httpRequest.Body.Seek(0, SeekOrigin.Begin);
            }

            return body;
        }

        /// <summary>
        ///     Used to validate a mac given the specified identity and body,
        ///     returns value indicating whether the given mac matched the
        ///     generated has with the parameters given.
        /// </summary>
        /// <param name="identity">The identity which maps to the secret which has been used.</param>
        /// <param name="body">The body of the request which was sent via the client.</param>
        /// <param name="mac">The mac which was sent via the client.</param>
        /// <param name="securityConfiguration">The security configuration taken from the application settings.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        private static bool ValidateData(string identity, string body, string mac, ISecurityConfiguration securityConfiguration)
        {
            identity = identity.ToLower();

            if (!securityConfiguration.IdentitySecrets.ContainsKey(identity))
            {
                return false;
            }

            var secret = securityConfiguration.IdentitySecrets[identity];
            var utcDateTime = DateTime.UtcNow;

            var lastHourHash = HashCompute.GenerateSha256Hash(
                body,
                utcDateTime.AddHours(-1).ToString("yyyyMMddHH"),
                secret);
            var currentHourHash = HashCompute.GenerateSha256Hash(body, utcDateTime.ToString("yyyyMMddHH"), secret);

            return lastHourHash.Equals(mac, StringComparison.OrdinalIgnoreCase)
                   || currentHourHash.Equals(mac, StringComparison.OrdinalIgnoreCase);
        }
    }
}
