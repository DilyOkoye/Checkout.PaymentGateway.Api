using System;
using System.Text.RegularExpressions;
using Checkout.PaymentGateway.Application.Contracts.Commands;
using Checkout.PaymentGateway.Domain.Entities;
using LanguageExt;
using static LanguageExt.Prelude;

namespace Checkout.PaymentGateway.Application.Validators
{
    public static class Validations
    {
        public static Validation<ErrorMsg, Guid> IsValidGuid(string str) =>
      Optional(str)
        .Where(s => !string.IsNullOrEmpty(s))
        .Where(s => Guid.TryParse(str, out _))
        .ToValidation<ErrorMsg>($"Id {str} must be a valid Guid")
        .Map(s => new Guid(s));

      
        private static bool IsAmountValid(decimal amount) 
        {
           bool value =  amount > 0;
            return value;
        }

        public static Validation<ErrorMsg, CreateTransactionCommand> TransactionDetailsMustBeValid(this CreateTransactionCommand self) 
        {
            var result = new Validation<ErrorMsg, CreateTransactionCommand>();
            // validate currency
            result = Optional(self)
            .Where(acc => acc.CurrencyIso != null)
            .ToValidation<ErrorMsg>("Currency must be set");
            if (result.IsFail)
            {
                return result;
            }
           
            result =  Optional(self)
            .Where(acc => acc.Amount != null)
            .Where(acc => IsAmountValid(acc.Amount))
            .ToValidation<ErrorMsg>("Amount must be set and be greater than zero");
            if (result.IsFail) 
            {
              return result;
            }

            // validate Card details
            result =  Optional(self)
           .Where(acc => acc.Card?.Number != null && acc.Card?.ExpiryDate != null && acc.Card?.Cvv != null)
           .Where(acc => IsCreditCardInfoValid(acc.Card.Number, acc.Card.ExpiryDate, acc.Card.Cvv))
           .ToValidation<ErrorMsg>("Card Information is not Valid");

            return result;
        }


        public static Validation<ErrorMsg, TransactionState> AccountMustExist(
        this TryOptionAsync<TransactionState> self) =>
        self
          .Match(
            Fail: _ => Fail<ErrorMsg, TransactionState>("Unable to get account info"),
            Some: Success<ErrorMsg, TransactionState>,
            None: () => Fail<ErrorMsg, TransactionState>("Account does not exist")
          )
          .Result;

        public static Validation<ErrorMsg, Unit> AccountMustNotExist(
    this TryOptionAsync<TransactionState> self) =>
    self
      .Match(
        Fail: _ => Fail<ErrorMsg, Unit>("Unable to get account info"),
        Some: acc => Fail<ErrorMsg, Unit>($"Id {acc.Id} already exists"),
        None: () => Success<ErrorMsg, Unit>(unit)
      )
      .Result;

        private static bool IsCreditCardInfoValid(string cardNo, string expiryDate, string cvv)
        {
            var cardCheck = new Regex(@"^(1298|1267|4512|4567|8901|8933|4165)([\-\s]?[0-9]{4}){3}$");
            var monthCheck = new Regex(@"^(0[1-9]|1[0-2])$");
            var yearCheck = new Regex(@"^20[0-9]{2}$");
            var cvvCheck = new Regex(@"^\d{3}$");

            if (!cardCheck.IsMatch(cardNo)) // <1>check card number is valid
                return false;
            if (!cvvCheck.IsMatch(cvv)) // <2>check cvv is valid as "999"
                return false;

            var dateParts = expiryDate.Split('/'); //expiry date in from MM/yyyy            
            if (!monthCheck.IsMatch(dateParts[0]) || !yearCheck.IsMatch(dateParts[1])) // <3 - 6>
                return false; // ^ check date format is valid as "MM/yyyy"

            var year = int.Parse(dateParts[1]);
            var month = int.Parse(dateParts[0]);
            var lastDateOfExpiryMonth = DateTime.DaysInMonth(year, month); //get actual expiry date
            var cardExpiry = new DateTime(year, month, lastDateOfExpiryMonth, 23, 59, 59);

            //check expiry greater than today & within next 6 years <7, 8>>
            return (cardExpiry > DateTime.Now && cardExpiry < DateTime.Now.AddYears(6));


        }
    }
}
