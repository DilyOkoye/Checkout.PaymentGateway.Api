using Checkout.PaymentGateway.Api.Controllers;
using Checkout.PaymentGateway.Api.Extensions;
using Checkout.PaymentGateway.Application.Contracts.Commands;
using Checkout.PaymentGateway.Application.Validators;
using Checkout.PaymentGateway.Application.ViewModels;
using Checkout.PaymentGateway.Domain.Entities;
using LanguageExt;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using static LanguageExt.Prelude;
using Assert = Xunit.Assert;
using CreateTransactionResponse = LanguageExt.Validation<
  Checkout.PaymentGateway.Application.Validators.ErrorMsg,
  LanguageExt.TryOptionAsync<Checkout.PaymentGateway.Application.ViewModels.TransactionViewModel>>;

namespace Checkout.PaymentGateway.Api.Tests
{
    public class ProcessPaymentTests
    {
        private readonly Mock<IMediator> _mediator;
        private readonly ProcessPayment _controller;

        public ProcessPaymentTests()
        {
            _mediator = new Mock<IMediator>();
            _controller = new ProcessPayment(_mediator.Object);
        }


        [Fact]
        public async Task ToActionResultBadRequest() =>
     Assert.NotNull(
       await Fail<ErrorMsg, TryOptionAsync<TransactionViewModel>>("fail")
         .ToActionResult()
       as BadRequestObjectResult
         );

        [Fact]
        public async Task ToActionResultOk() =>
      Assert.NotNull(
        await Success<ErrorMsg, TryOptionAsync<TransactionViewModel>>(
            TryOptionAsync(TransactionViewModel.New(
              TransactionState.New(TransactionState.GenerateTransaction()))))
          .ToActionResult()
        as OkObjectResult
      );

        [Fact]
        public async Task ToActionResultNotFound() =>
      Assert.NotNull(
        (await Success<ErrorMsg, TryOptionAsync<TransactionViewModel>>(
            TryOptionAsync<TransactionViewModel>(None))
          .ToActionResult())
        as NotFoundResult
      );

        [Fact]
        public async Task ToActionResultServerError() =>
          Assert.NotNull(
            await Success<ErrorMsg, TryOptionAsync<TransactionViewModel>>(
                  TryOptionAsync<TransactionViewModel>(new Exception("fail")))
                .ToActionResult()
            as StatusCodeResult
          );


        [Fact]
        public async Task RetrievePayment()
        {

            _mediator
              .Setup(m => m.Send(
                It.IsAny<IRequest<CreateTransactionResponse>>(),
                default))
              .ReturnsAsync(
                Success<ErrorMsg, TryOptionAsync<TransactionViewModel>>(
                  TryOptionAsync(TransactionViewModel.New(
                    TransactionState.New(TransactionState.GenerateTransaction())))
                ));

            var result = await _controller.RetrievePayment("-- id --");

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task NewPayment()
        {
            _mediator
              .Setup(m => m.Send(
                It.IsAny<IRequest<CreateTransactionResponse>>(),
                default))
              .ReturnsAsync(
                Success<ErrorMsg, TryOptionAsync<TransactionViewModel>>(
                  TryOptionAsync(TransactionViewModel.New(
                    TransactionState.New(TransactionState.GenerateTransaction())))
                ));

            var result = await _controller.NewPayment(new CreateTransactionCommand());

            Assert.IsType<OkObjectResult>(result);
        }



       

    }

}
