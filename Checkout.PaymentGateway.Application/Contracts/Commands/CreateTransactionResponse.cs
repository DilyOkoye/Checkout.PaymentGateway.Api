using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkout.PaymentGateway.Application.Contracts.Commands
{
    public class CreateTransactionResponse
    {
        public Guid PaymentId { get; set; }
        public string Status { get; set; }
    }
}
