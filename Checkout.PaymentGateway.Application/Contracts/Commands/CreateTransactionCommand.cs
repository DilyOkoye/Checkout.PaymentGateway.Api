using Checkout.PaymentGateway.Application.Validators;
using Checkout.PaymentGateway.Application.ViewModels;
using Checkout.PaymentGateway.Domain.Entities;
using LanguageExt;
using MediatR;

namespace Checkout.PaymentGateway.Application.Contracts.Commands
{
    public class CreateTransactionCommand : Record<CreateTransactionCommand>,
        IRequest<Validation<ErrorMsg, TryOptionAsync<TransactionViewModel>>>
    {
        public Guid Id { get; set; }
        public Guid MerchantId { get; set; }
        public decimal Amount { get; set; }
        public string Payload { get; set; }
        public DateTime PaymentDate { get; set; }
        public string CurrencyIso { get; set; }
        public Card? Card { get; set; }
        public string CountryCode { get; set; }
        public string Description { get; set; }
    }
}
