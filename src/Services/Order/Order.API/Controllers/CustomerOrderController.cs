using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Order.API.Services;
using Order.API.ViewModels.Order.Requests;
using Order.API.ViewModels.Order.Responses;
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
        public async Task Payment()
        {
            return await _orderService.PaymentAsync();
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
