using Microsoft.Extensions.Caching.Memory;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkout.PaymentGateway.Persistence.Tests
{
    public class TransactionRepositoryTest
    {
        [Test]
        public void IMemoryCacheWithMock()
        {
            var url = "testUrl";
            var response = "json string";
            var memoryCache = Mock.Of<IMemoryCache>();
            var cachEntry = Mock.Of<ICacheEntry>();

            var mockMemoryCache = Mock.Get(memoryCache);
            mockMemoryCache
                .Setup(m => m.CreateEntry(It.IsAny<object>()))
                .Returns(cachEntry);

            var cachedResponse = memoryCache.Set<string>(url, response);

            Assert.IsNotNull(cachedResponse);
            Assert.AreEqual(response, cachedResponse);
        }
    }
}
