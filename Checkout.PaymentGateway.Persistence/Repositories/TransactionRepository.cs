using Checkout.PaymentGateway.Domain.Entities;
using Checkout.PaymentGateway.Domain.Exceptions;
using LanguageExt;
using Microsoft.Extensions.Caching.Memory;
using static LanguageExt.Prelude;

namespace Checkout.PaymentGateway.Persistence.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly IMemoryCache _cache;

        public TransactionRepository(IMemoryCache cache)
        {
            _cache = cache;
        }

        public TryOptionAsync<TransactionState> GetTransactionAsync(Guid id)
        {
            bool paymentFound = _cache.TryGetValue<TransactionState>(id, out var payment);
            return TryOptionAsync(paymentFound ? payment : null);
        }

        public TryOptionAsync<TransactionState> CreateTransactionAsync(TransactionState transaction)
        {
            if (_cache.TryGetValue<TransactionState>(transaction.Id, out var existingPayment))
                throw new DuplicateTransactionException($"Duplicate transaction with id {transaction.Id}");

            return   TryOptionAsync(_cache.Set(transaction.Id, transaction));
        }

        public TryOptionAsync<TransactionState> UpdateTransactionAsync(TransactionState transaction)
        {
            if (!_cache.TryGetValue<TransactionState>(transaction.Id, out var existingPayment))
                throw new ArgumentException($"No payment found with id {transaction.Id}");

            return TryOptionAsync(_cache.Set(transaction.Id, transaction));
        }

    }
}
