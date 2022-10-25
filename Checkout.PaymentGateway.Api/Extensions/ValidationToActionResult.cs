using Checkout.PaymentGateway.Application.Contracts.Commands;
using Checkout.PaymentGateway.Application.Validators;
using LanguageExt;
using Microsoft.AspNetCore.Mvc;
using static LanguageExt.Prelude;

namespace Checkout.PaymentGateway.Api.Extensions
{
    public static class ValidationToActionResult
    {
        public static Task<IActionResult> ToActionResult<T>(
     this Validation<ErrorMsg, TryOptionAsync<T>> self) =>
     self.Match(
       Fail: e => Task.FromResult<IActionResult>(
         new BadRequestObjectResult(e)),
       Succ: valid => valid.Match<T, IActionResult>(
         Some: r => new OkObjectResult(r),
         None: () => new NotFoundResult(),
         Fail: _ => new StatusCodeResult(
           StatusCodes.Status500InternalServerError)
       )
     );
      
    }


}
