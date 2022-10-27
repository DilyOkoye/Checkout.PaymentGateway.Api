using Checkout.PaymentGateway.Application.Validators;
using Checkout.PaymentGateway.Application.ViewModels;
using LanguageExt;
using MediatR;

namespace Checkout.PaymentGateway.Application.Queries
{

    public class GetPaymentById : Record<GetPaymentById>,
   IRequest<Validation<ErrorMsg, TryOptionAsync<TransactionViewModel>>>
    {
        public GetPaymentById(string transactionId)
        {
            TransactionId = transactionId;
        }

        public string TransactionId { get; }
    }
}
