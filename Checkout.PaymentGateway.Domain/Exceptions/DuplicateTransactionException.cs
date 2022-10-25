using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkout.PaymentGateway.Domain.Exceptions
{
   
    public class DuplicateTransactionException : Exception
    {
        public DuplicateTransactionException()
        {
        }

        public DuplicateTransactionException(string message)
            : base(message)
        {
        }

        public DuplicateTransactionException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

}
