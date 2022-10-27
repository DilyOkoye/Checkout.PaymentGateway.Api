using Checkout.PaymentGateway.Application.Contracts.Commands;
using Checkout.PaymentGateway.Domain.Entities;
using Xunit;
using static Checkout.PaymentGateway.Application.Validators.Validations;
using static LanguageExt.Prelude;

namespace Checkout.PaymentGateway.Application.Tests
{
    public class ValidatorsTests
    {
        [Fact]
        public void IsValidGuidEmpty() =>
      Assert.True(
        IsValidGuid(string.Empty)
          .IsFail
      );

        [Fact]
        public void IsValidGuidFail() =>
          Assert.True(
            IsValidGuid("-- invalid --")
              .IsFail
          );

        [Fact]
        public void IsValidGuidSuccess() =>
          Assert.True(
            IsValidGuid("669dceb5-107d-4701-ae9c-802d6963d081")
              .IsSuccess
          );

        [Fact]
        public void AccountMustNotExist() =>
       Assert.True(
         TryOptionAsync(() => Task.FromResult(
             TransactionState.New(TransactionState.GenerateTransaction())))
           .AccountMustNotExist()
           .IsFail
       );

        [Fact]
        public void TransactionDetailsMustBeValidSuccess() =>

        Assert.True(
       new CreateTransactionCommand
       {
           Amount = 20,
           Id = Guid.NewGuid(),
           MerchantId = Guid.NewGuid(),
           CountryCode = "GB",
           PaymentDate = DateTime.UtcNow,
           CurrencyIso = "GBP",
           Payload = "Test",
           Description = "Test",
           Card = new Card { BillingInformation = "Test", Cvv = "322", ExpiryDate = "01/2027", HolderName = "Dili Okoye", Number = "4165555555554444" }
       }
         .TransactionDetailsMustBeValid()
         .IsSuccess
     );
    }


}
