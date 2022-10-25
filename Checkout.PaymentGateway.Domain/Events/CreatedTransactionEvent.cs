using Checkout.PaymentGateway.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkout.PaymentGateway.Domain.Events
{
 

    public class CreatedTransactionEvent : ITransactionEvent
    {
        public TransactionState Transactions { get; private set; }

        public CreatedTransactionEvent(TransactionState transactions)
        {
            Transactions = transactions;
        }
    }
}
