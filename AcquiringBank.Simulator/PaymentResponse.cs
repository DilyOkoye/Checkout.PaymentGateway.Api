using System;
using System.Collections.Generic;
using System.Text;

namespace AcquiringBank.Simulator
{
   
    public class PaymentResponse
    {
        public Guid PaymentReference { get; private set; }
        public DeclineCodes? Reason { get; private set; }
        public TransactionStatus Status { get; private set; }

        public PaymentResponse(TransactionStatus status, Guid paymentReference, DeclineCodes? reason = null)
        {
            Status = status;
            Reason = reason;
            PaymentReference = paymentReference;
        }

    }
}
