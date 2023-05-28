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
        public async Task<OrderConfirmationResponse> Submit([FromBody] OrderConfirmationRequest request)
        {
            return await _orderService.SubmitAsync(request);
        }

        [HttpGet()]
        public async Task<List<OrderHistoryItemReponse>> GetOrders()
        {
            return await _orderService.GetOrdersAsync();
        }
    }
}
