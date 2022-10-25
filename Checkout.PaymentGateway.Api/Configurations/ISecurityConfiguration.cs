namespace Checkout.PaymentGateway.Api.Configurations
{
    /// <summary> Used to hold the state of the security configuration from the application settings.</summary>
    public interface ISecurityConfiguration
    {
        /// <summary>Gets or sets a value indicating whether the security feature is enabled.</summary>
        public bool IsEnabled { get; set; }

        /// <summary>Gets or sets the dictionary holding the values of identities and there associated secrets.</summary>
        public Dictionary<string, string> IdentitySecrets { get; set; }

    }
}
