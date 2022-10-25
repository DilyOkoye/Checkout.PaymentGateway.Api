using AcquiringBank.Simulator;
using Checkout.PaymentGateway.Application.Contracts.Commands;
using Checkout.PaymentGateway.Application.EventHandlers;
using Checkout.PaymentGateway.Application.Validators;
using Checkout.PaymentGateway.Application.ViewModels;
using Checkout.PaymentGateway.Domain.Entities;
using Checkout.PaymentGateway.Domain.Events;
using Checkout.PaymentGateway.Persistence.Repositories;
using LanguageExt;
using MediatR;
using Microsoft.Extensions.Logging;
using static Checkout.PaymentGateway.Application.Validators.Validations;
using static LanguageExt.Prelude;
using CreateTransactionResponse = LanguageExt.Validation<
  Checkout.PaymentGateway.Application.Validators.ErrorMsg,
  LanguageExt.TryOptionAsync<Checkout.PaymentGateway.Application.ViewModels.TransactionViewModel>>;

namespace Checkout.PaymentGateway.Application.CommandHandlers
{
    public class CreateTransactionHandler : IRequestHandler<CreateTransactionCommand, CreateTransactionResponse>
    {
        private readonly ITransactionEventHandler _transactionEventHandler;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ILogger<CreateTransactionHandler> _logger;

        public CreateTransactionHandler(ITransactionEventHandler transactionEventHandler,ITransactionRepository transactionRepository, ILogger<CreateTransactionHandler> logger)
        {
            _transactionEventHandler = transactionEventHandler;
            _transactionRepository = transactionRepository;
            _logger = logger;
        }


        public Task<CreateTransactionResponse> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            var result = (IsValidGuid(request.Id.ToString()), request.TransactionDetailsMustBeValid())
          .Apply((id, trans) =>
            _transactionRepository.GetTransactionAsync(id).AccountMustNotExist()
              .Map(_ => PersistPayment(
                TransactionState.New(CreateTransactionState(request)))))
          .Bind(resp => resp)
          .AsTask();

            _transactionEventHandler.PublishAsync(new CreatedTransactionEvent(CreateTransactionState(request))); 
            return result;
        }

        private TransactionState CreateTransactionState(CreateTransactionCommand request)
        {
            return new TransactionState(request.Id, request.MerchantId, request.Amount, request.Payload, request.PaymentDate, request.CurrencyIso, request.Card,request.CountryCode,request.Description, nameof(TransactionStatus.Settling));
        }

        private TryOptionAsync<TransactionViewModel> PersistPayment(
        TransactionState state) =>
        from st in _transactionRepository.CreateTransactionAsync(state)
        select TransactionViewModel.New(st);



    }
}
