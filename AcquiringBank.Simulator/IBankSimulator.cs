using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AcquiringBank.Simulator
{
    public interface IBankSimulator
    {
        Task<PaymentResponse> PostTransactionAsync(PaymentRequest request);

    }

}
