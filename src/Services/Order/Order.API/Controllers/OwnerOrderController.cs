using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Order.API.Services;
using Order.API.ViewModels.Dashboard.Response;
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
        public async Task<List<OrderHistoryItemReponse>> GetOrders([FromQuery] string keyname, [FromQuery] int? pitchType, [FromQuery] bool isDashboard)
        {
            return await _service.GetOrdersAsync(keyname, pitchType, isDashboard);
        }

        [HttpGet("customer")]
        public async Task<List<CustomerItemReponse>> GetCustomers([FromQuery] string keyname)
        {
            return await _service.GetCustomersAsync(keyname);
        }

        [HttpGet("dashboard/pitchType")]
        public async Task<List<PitchTypeDashboardModel>> GetPitchTypeDashboard()
        {
            return await _service.GetPitchTypeDashboardAsync();
        }

        [HttpGet("dashboard/pitchName")]
        public async Task<List<PitchNameDashboardModel>> GetPitchNameDashboard()
        {
            return await _service.GetPitchNameDashboardAsync();
        }

        [HttpGet("dashboard/month")]
        public async Task<List<RevanueByMonthModel>> GetRevanueByMonth()
        {
            return await _service.RevanueByMonthModelAsync();
        }
    }
}
