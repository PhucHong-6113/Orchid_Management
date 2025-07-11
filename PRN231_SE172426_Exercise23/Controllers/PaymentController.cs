using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.OrderService;
using Service.PaymentService;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PRN231_SE172426_Exercise23.Controllers
{
    [ApiController]
    [Route("api/payments")]
    public class PaymentController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IVNPayService _vnPayService;

        public PaymentController(IOrderService orderService, IVNPayService vnPayService)
        {
            _orderService = orderService;
            _vnPayService = vnPayService;
        }

        [HttpPost("create/{orderId}")]
        [Authorize]
        public async Task<IActionResult> CreatePayment(Guid orderId)
        {
            try
            {
                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                var returnUrl = $"{baseUrl}/api/payments/vnpay-return";

                var response = await _orderService.CreatePaymentForOrderAsync(orderId, returnUrl);

                if (!response.Success)
                {
                    return BadRequest(new { message = response.Message });
                }

                return Ok(new
                {
                    paymentUrl = response.PaymentUrl
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("vnpay-return")]
        public async Task<IActionResult> PaymentReturn()
        {
            var returnRequest = await _vnPayService.ProcessReturnRequest(Request.Query);

            await _orderService.ProcessPaymentReturnAsync(returnRequest);

            // For successful payment
            if (returnRequest.ResponseCode == "00")
            {
                // You can customize this to redirect to a success page on your frontend
                return Redirect($"/payment-success?orderId={returnRequest.OrderId}");
            }

            // For failed payment
            return Redirect($"/payment-failed?orderId={returnRequest.OrderId}&code={returnRequest.ResponseCode}");
        }
    }
}