using AcquiringBank.Simulator;

namespace Checkout.PaymentGateway.Domain.Entities
{


    public enum CurrencyCode
    { GBP, USD, EUR }

    public class TransactionState
    {
        public Guid Id { get; set; }
        public Guid MerchantId { get; set; }
        public decimal Amount { get; set; }
        public string Payload { get; set; }
        public DateTime PaymentDate { get; set; }
        public string CurrencyIso { get; set; }
        public Card CardInfo { get; set; }
        public string Status { get; set; }
        public string CountryCode { get; set; }
        public string Description { get; set; }


        public TransactionState(
          Guid Id,
          Guid MerchantId,
          decimal Amount,
          string Payload,
          DateTime PaymentDate,
          string CurrencyIso,
          Card CardInfo,
          string CountryCode,
          string description,
          string Status
)
        {
            this.Id = Id;
            this.MerchantId = MerchantId;
            this.Amount = Amount;
            this.Payload = Payload;
            this.PaymentDate = PaymentDate;
            this.CurrencyIso = CurrencyIso;
            this.CardInfo = CardInfo;
            this.CountryCode = CountryCode;
            Description = description;
            this.Status = Status;

        }


        public static TransactionState New(TransactionState state)
        {
            return new(state.Id, state.MerchantId, state.Amount, state.Payload, state.PaymentDate, state.CurrencyIso, state.CardInfo,state.CountryCode,state.Description, state.Status);
        }

        public static TransactionState GenerateTransaction()
        {
            Card card = new Card();
            card.BillingInformation = "Test";
            card.Cvv = "322";
            card.ExpiryDate = "01/2027";
            card.HolderName = "Dili Okoye";
            card.Number = "2345678823570000";

            var Id = Guid.NewGuid();
            var MerchantId = Guid.NewGuid();
            var Amount = 30;
            var payload = "Test";
            var PaymentDate = DateTime.UtcNow;
            var CurrencyIso = "GBP";
            var CountryCode = "GB";
            var Description = "Test";
            var Status = "Settled";


            return new(Id, MerchantId, Amount, payload, PaymentDate, CurrencyIso, card, CountryCode, Description, Status);
        }
    }
}
