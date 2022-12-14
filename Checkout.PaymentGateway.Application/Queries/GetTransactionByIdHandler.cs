using Checkout.PaymentGateway.Application.ViewModels;
using Checkout.PaymentGateway.Persistence.Repositories;
using LanguageExt;
using MediatR;
using static Checkout.PaymentGateway.Application.Validators.Validations;
using CreateTransactionResponse = LanguageExt.Validation<
  Checkout.PaymentGateway.Application.Validators.ErrorMsg,
  LanguageExt.TryOptionAsync<Checkout.PaymentGateway.Application.ViewModels.TransactionViewModel>>;


namespace Checkout.PaymentGateway.Application.Queries
{

    public class GetTransactionByIdHandler :
    IRequestHandler<GetPaymentById, CreateTransactionResponse>
    {
        private readonly ITransactionRepository _transactionRepository;

        public GetTransactionByIdHandler(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public Task<CreateTransactionResponse> Handle(
          GetPaymentById request,
          CancellationToken cancellationToken) =>
          IsValidGuid(request.TransactionId)
            .Bind<TryOptionAsync<TransactionViewModel>>(id =>
              _transactionRepository.GetTransactionAsync(id)
                .Map(TransactionViewModel.New)
            )
          .AsTask();
    }
}
