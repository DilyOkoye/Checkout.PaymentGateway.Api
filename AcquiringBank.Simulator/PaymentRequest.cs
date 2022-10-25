using System;
using System.Collections.Generic;
using System.Text;

namespace AcquiringBank.Simulator
{
    public class PaymentRequest
    {
        public Guid PaymentRef { get; set; }
        public string HoldersName { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string CardNumber { get; set; }
        public string CVV { get; set; }
        public string ExpiryDate { get; set; }
        public string Narration { get; set; }
        public string CountryCode { get; set; }
        public Guid MerchantId { get; set; }
    }
}
