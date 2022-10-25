using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AcquiringBank.Simulator
{
    public class BankSimulator: IBankSimulator
    {

        public async Task<PaymentResponse> PostTransactionAsync(PaymentRequest request)
        {
            // Decline Blacklisted cards due to reported stolen card
            if (CheckBlacklistedCard(request.CardNumber))
                return await Task.FromResult(new PaymentResponse(TransactionStatus.Declined, request.PaymentRef, DeclineCodes.SuspectedFraud));

            if (CheckBlacklistedCard(request.CardNumber))
                return await Task.FromResult(new PaymentResponse(TransactionStatus.Declined, request.PaymentRef, DeclineCodes.SuspectedFraud));

            return await Task.FromResult(new PaymentResponse(TransactionStatus.Settled, request.PaymentRef, null));
        }

        public bool CheckBlacklistedCard(string card) 
        {
            return (Helpers.BlacklistedCards.Contains(card));
        }

        public bool CheckAllowableLocations(string countryCode)
        {
            return (Helpers.BlacklistedCards.Contains(countryCode));
        }
      


    }
}
