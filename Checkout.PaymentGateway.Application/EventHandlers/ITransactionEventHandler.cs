using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkout.PaymentGateway.Application.EventHandlers
{

    public interface ITransactionEventHandler
    {
        /// <summary>
        /// Publishes an event
        /// </summary>
        /// <param name="Event">The event</param>
        Task PublishAsync<ITransactionEvent>(ITransactionEvent Event);
    }
}
