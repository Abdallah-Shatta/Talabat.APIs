using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Entities.OrderAggregate;
using Talabat.Core.IServices;

namespace Talabat.APIs.Controllers
{
    [Authorize]
    public class PaymentsController : BaseAPIController
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [ProducesResponseType(typeof(CustomerBasket), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var basket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);

            return basket is null ? NotFound(new ApiResponse(404, "Error with your basket")) : Ok(basket);
        }

        [AllowAnonymous]
        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var stripeEvent = EventUtility.ParseEvent(json);

            PaymentIntent paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
            Order order;

            switch (stripeEvent.Type)
            {
                case EventTypes.PaymentIntentSucceeded:
                    order = await _paymentService.UpdatePaymentIntent(paymentIntent.Id, true);
                break;

                case EventTypes.PaymentIntentPaymentFailed:
                    order = await _paymentService.UpdatePaymentIntent(paymentIntent.Id, false);
                break;
            }

            return Ok();
        }
    }
}
