using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Order.API.Services;
using Order.API.ViewModels.Order.Requests;
using Order.API.ViewModels.Order.Responses;
using Shared.API.Identity;

namespace Order.API.Controllers
{
    [Route("api/order")]
    [Authorize(PolicyNames.Customer_API)]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost()]
        public async Task<OrderConfirmationResponse> Submit([FromBody] OrderConfirmationRequest request)
        {
            return await _orderService.SubmitAsync(request);
        }
    }
}
