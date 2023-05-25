using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pitch.API.Services;
using Pitch.API.ViewModels.Store.Requests;
using Pitch.API.ViewModels.Store.Responses;
using Shared.API.Identity;

namespace Pitch.API.Controllers
{
    [Route("api/pitch/store-ordering")]
    [Authorize(PolicyNames.Customer_API)]
    public class StoreOrderingController
    {
        private readonly StoreOrderingService _storeOrderingService;

        public StoreOrderingController(StoreOrderingService storeOrderingService)
        {
            _storeOrderingService = storeOrderingService;
        }

        [HttpGet()]
        public async Task<List<StoreOrderingItemResponse>> GetStores([FromBody] GetStoreOrderingRequest request)
        {
            return await _storeOrderingService.GetStoresAsync(request);
        }

        [HttpGet("{id}")]
        public async Task<StoreOrderingDetailResponse> GetStore([FromRoute] int id)
        {
            return await _storeOrderingService.GetStoreAsync(id);
        }
    }
}
