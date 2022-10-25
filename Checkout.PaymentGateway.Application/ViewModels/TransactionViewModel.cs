using AcquiringBank.Simulator;
using Checkout.PaymentGateway.Domain.Entities;
using LanguageExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkout.PaymentGateway.Application.ViewModels
{
  
    public class TransactionViewModel : Record<TransactionViewModel>
    {
        public Guid Id { get; set; }
        internal Guid MerchantId { get; set; }
        public decimal Amount { get; set; }
        public string Payload { get; set; }
        public DateTime PaymentDate { get; set; }
        public string CurrencyIso { get; set; }
        public Card CardInfo { get; set; }
        public string Status { get; set; }


        public TransactionViewModel(TransactionState state)
        {
            Id = state.Id;
            MerchantId = state.MerchantId;
            Amount = state.Amount;
            Payload = state.Payload;
            PaymentDate = state.PaymentDate;
            CurrencyIso = state.CurrencyIso;
            CardInfo = state.CardInfo;
            Status = state.Status;
        }

        public static TransactionViewModel New(TransactionState state) =>
          new(state);
    }
}
