using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkout.PaymentGateway.Application.EventHandlers
{

    public interface ITransactionEventHandler
    {
        Task PublishAsync<ITransactionEvent>(ITransactionEvent Event);
    }
}
