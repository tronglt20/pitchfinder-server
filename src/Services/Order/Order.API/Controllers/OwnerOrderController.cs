using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Order.API.Services;
using Order.API.ViewModels.Order.Responses;
using Shared.API.Identity;

namespace Order.API.Controllers
{
    [Route("api/order/owner")]
    [Authorize(PolicyNames.OWNER_API)]
    public class OwnerOrderController : ControllerBase
    {
        private readonly OwnerOrderService _service;

        public OwnerOrderController(OwnerOrderService service)
        {
            _service = service;
        }

        [HttpGet()]
        public async Task<List<OrderHistoryItemReponse>> GetOrders()
        {
            return await _service.GetOrdersAsync();
        }
    }
}
