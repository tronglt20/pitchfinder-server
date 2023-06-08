using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Order.API.Services;
using Order.API.ViewModels.Order.Requests;
using Order.API.ViewModels.Order.Responses;
using Order.Infrastructure.Dtos;
using Shared.API.Identity;

namespace Order.API.Controllers
{
    [Route("api/order/customer")]
    [Authorize(PolicyNames.Customer_API)]
    public class CustomerOrderController : ControllerBase
    {
        private readonly CustomerOrderService _orderService;

        public CustomerOrderController(CustomerOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost()]
        public async Task SendPaymentRequest()
        {
            await _orderService.SendPaymentRequestAsync();
        }

        [HttpGet("payment-result")]
        public async Task ReceivePaymentResult([FromQuery] MomoPaymentResult paymentResult)
        {
            await _orderService.ReceivePaymentResultAsync(paymentResult);
        }

        [HttpPost()]
        public async Task<OrderConfirmationResponse> MakeOrder([FromBody] OrderConfirmationRequest request)
        {
            return await _orderService.MakeOrderAsync(request);
        }

        [HttpGet()]
        public async Task<List<OrderHistoryItemReponse>> GetOrders()
        {
            return await _orderService.GetOrdersAsync();
        }
    }
}
