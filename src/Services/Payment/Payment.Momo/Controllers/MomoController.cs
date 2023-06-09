using Microsoft.AspNetCore.Mvc;
using Payment.Momo.Services;
using Payment.Momo.ViewModels;

namespace Payment.Momo.Controllers
{
    [Route("api/payment/momo")]
    public class MomoController : ControllerBase
    {
        private readonly MomoService _momoService;

        public MomoController(MomoService momoService)
        {
            _momoService = momoService;
        }

        [HttpPost()]
        public async Task<string> Payment([FromBody] OrderCreatedRequest request)
        {
            return await _momoService.PaymentAsync(request.OrderId, request.Amount);
        }

        [HttpGet("result")]
        public async Task ReceivePaymentResult([FromQuery] MomoPaymentResult paymentResult)
        {
            await _momoService.ReceivePaymentResultAsync(paymentResult);
        }
    }
}
