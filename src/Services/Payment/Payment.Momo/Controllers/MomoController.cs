using Microsoft.AspNetCore.Mvc;
using Payment.Momo.Services;

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
        public async Task<string> Payment()
        {
            return await _momoService.PaymentAsync(Guid.NewGuid().ToString(), "5000");
        }
    }
}
