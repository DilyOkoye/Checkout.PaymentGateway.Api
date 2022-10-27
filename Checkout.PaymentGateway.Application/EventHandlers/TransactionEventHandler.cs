using AcquiringBank.Simulator;
using Checkout.PaymentGateway.Domain.Entities;
using Checkout.PaymentGateway.Domain.Events;
using Checkout.PaymentGateway.Persistence.Repositories;

namespace Checkout.PaymentGateway.Application.EventHandlers
{

    public class TransactionEventHandler : ITransactionEventHandler
    {
        private readonly IBankSimulator _bankSimulator;
        private readonly ITransactionRepository _transactionRepository;

        public TransactionEventHandler(ITransactionRepository transactionRepository, IBankSimulator bankSimulator)
        {
            _transactionRepository = transactionRepository;
            _bankSimulator = bankSimulator;
        }

        public Task PublishAsync<ITransactionEvent>(ITransactionEvent Event)
        {
            var task = Task.Run(() => ProcessPayment((Event as CreatedTransactionEvent).Transactions));//fire and forget

            return Task.CompletedTask;
        }

        public async Task ProcessPayment(TransactionState transaction)
        {
            var request = CreatePaymentRequest(transaction);
            var response = await _bankSimulator.PostTransactionAsync(request);
            transaction.Status = response.Status.ToString();
            await _transactionRepository.UpdateTransactionAsync(transaction);
        }

        public PaymentRequest CreatePaymentRequest(TransactionState transaction)
        {
            return new PaymentRequest
            {
                Amount = transaction.Amount,
                CardNumber = transaction.CardInfo.Number,
                CountryCode = transaction.CountryCode,
                Currency = transaction.CurrencyIso,
                CVV = transaction.CardInfo.Cvv,
                ExpiryDate = transaction.CardInfo.ExpiryDate,
                HoldersName = transaction.CardInfo.HolderName,
                Narration = transaction.Description,
                PaymentRef = transaction.Id,
                MerchantId = transaction.MerchantId,

            };

        }
    }
}
