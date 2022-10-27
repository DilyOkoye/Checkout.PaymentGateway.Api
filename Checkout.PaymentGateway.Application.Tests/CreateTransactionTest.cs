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
using Checkout.PaymentGateway.Application.CommandHandlers;
using Checkout.PaymentGateway.Application.EventHandlers;
using Checkout.PaymentGateway.Application.Contracts.Commands;

namespace Checkout.PaymentGateway.Application.Tests
{

    public class CreateTransactionTest
    {
        private readonly Mock<ITransactionRepository> _transactionRepository;
        private readonly Mock<ITransactionEventHandler> _transactionEventHandler;
        private readonly CreateTransactionHandler _handler;

        public CreateTransactionTest()
        {
            _transactionRepository = new Mock<ITransactionRepository>();
            _transactionEventHandler = new Mock<ITransactionEventHandler>();
            _handler = new CreateTransactionHandler(_transactionEventHandler.Object,_transactionRepository.Object);
        }

        [Fact]
        public async Task HandleCreatedTransaction()
        {
            _transactionRepository
              .Setup(m => m.GetTransactionAsync(It.IsAny<Guid>()))
              .Returns(TryOptionAsync<TransactionState>(None));
            _transactionRepository
              .Setup(m => m.CreateTransactionAsync(It.IsAny<TransactionState>()))
              .Returns(TryOptionAsync(Task.FromResult(
                TransactionState.New(TransactionState.GenerateTransaction())))
              );

            var result = await _handler.Handle(new CreateTransactionCommand
            {
                Amount = 20,
                Id = Guid.NewGuid(),
                MerchantId = Guid.NewGuid(),
                CountryCode = "GB",
                PaymentDate = DateTime.UtcNow,
                CurrencyIso = "GBP",
                Payload = "Test",
                Description = "Test",
                Card = new Card { BillingInformation = "Test", Cvv = "322", ExpiryDate = "01/2027", HolderName = "Dili Okoye", Number = "4165678823570000" }

            }, default);

            Assert.True(result.IsSuccess);
        }


        [Fact]
        public async Task HandleFailedTransaction() =>
          Assert.True(
            (await _handler.Handle(new CreateTransactionCommand
            {
            }, default))
            .IsFail
          );
    }
}
