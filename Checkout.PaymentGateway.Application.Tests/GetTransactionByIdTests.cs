using Microsoft.Extensions.Logging;
using Checkout.PaymentGateway.Application.Queries;
using Checkout.PaymentGateway.Persistence.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static LanguageExt.Prelude;
using Checkout.PaymentGateway.Domain.Entities;

namespace Checkout.PaymentGateway.Application.Tests
{

    public class GetTransactionByIdTests
    {
        private readonly Mock<ITransactionRepository> _repo;
        private readonly GetTransactionByIdHandler _handler;

        public GetTransactionByIdTests()
        {
            _repo = new Mock<ITransactionRepository>();
            _handler = new GetTransactionByIdHandler(_repo.Object);
        }

        [Fact]
        public async Task HandleGetTransactionById()
        {
            var payment = TransactionState.GenerateTransaction();
            _repo
              .Setup(m => m.GetTransactionAsync(It.IsAny<Guid>()))
              .Returns(TryOptionAsync(Task.FromResult(
                TransactionState.New(payment)))
              );

            var result = await _handler.Handle(
              new GetPaymentById(payment.Id.ToString()),
              default);

            Assert.True(result.IsSuccess);
        }
    }
}
