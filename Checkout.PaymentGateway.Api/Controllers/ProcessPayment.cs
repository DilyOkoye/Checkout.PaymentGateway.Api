using Checkout.PaymentGateway.Api.Filters;
using LanguageExt;
using Checkout.PaymentGateway.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Checkout.PaymentGateway.Application.Contracts.Commands;
using Checkout.PaymentGateway.Application.Queries;

namespace Checkout.PaymentGateway.Api.Controllers
{
    [ApiController]
    [Route("Payments")]
    [SecurityFilter]

    public class ProcessPayment : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProcessPayment(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost]
         public async Task<IActionResult> NewPayment([FromBody] CreateTransactionCommand command) =>
         await _mediator
        .Send(command)
        .Bind(acc => acc.ToActionResult());

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> RetrievePayment(string id) =>
        await _mediator
        .Send(new GetPaymentById(id))
        .Bind(acc => acc.ToActionResult());

    }
}

