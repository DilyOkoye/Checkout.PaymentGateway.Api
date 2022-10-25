namespace Checkout.PaymentGateway.Domain.Entities
{
    public class Card
    {
        public string HolderName { get; set; }
        public string Number { get; set; }
        public string ExpiryDate { get; set; }
        public string Cvv { get; set; }
        public string BillingInformation { get; set; }
    }
}