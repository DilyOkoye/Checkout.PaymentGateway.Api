using LanguageExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkout.PaymentGateway.Domain.Entities
{
    public class Transactions : Record<Transactions>
    {
        public Guid Id { get; private set; }
        public Guid MerchantId { get; private set; }
        public Card Card { get; private set; }

        public Transactions(Guid id, Guid MerchantId, Card card)
        {
            Id = id;
            MerchantId = MerchantId;
            Card = card;
        }
    }
}
