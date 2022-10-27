using Checkout.PaymentGateway.Api.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Checkout.PaymentGateway.Api.Tests
{
    public class HelperTests
    {
        [Fact]
        public void HashComputeTest()
        {
            var message = "test message";
            var dateTimeStamp = DateTime.UtcNow.ToString();
            var secret = "test secret";
            var result = HashCompute.GenerateSha256Hash(message, dateTimeStamp, secret);
            Assert.NotNull(result);
        }
    }
}
