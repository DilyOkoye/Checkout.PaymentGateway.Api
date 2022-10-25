using Checkout.PaymentGateway.Domain.Entities;
using LanguageExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Checkout.PaymentGateway.Persistence.Repositories
{

    public interface ITransactionRepository
    {
        TryOptionAsync<TransactionState> GetTransactionAsync(Guid id);

        TryOptionAsync<TransactionState> CreateTransactionAsync(TransactionState transaction);

        TryOptionAsync<TransactionState> UpdateTransactionAsync(TransactionState transaction);

    }
}


