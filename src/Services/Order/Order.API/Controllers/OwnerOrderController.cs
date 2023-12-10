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
        public async Task<List<OrderHistoryItemReponse>> GetOrders([FromQuery] string keyname, [FromQuery] int? pitchType)
        {
            return await _service.GetOrdersAsync(keyname, pitchType);
        }

        [HttpGet("customer")]
        public async Task<List<CustomerItemReponse>> GetCustomers([FromQuery] string keyname)
        {
            return await _service.GetCustomersAsync(keyname);
        }
    }
}
