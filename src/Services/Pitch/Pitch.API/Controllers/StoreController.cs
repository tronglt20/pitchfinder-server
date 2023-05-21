using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pitch.API.Services;
using Pitch.API.ViewModels.Store.Requests;
using Pitch.API.ViewModels.Store.Responses;

namespace Pitch.API.Controllers
{
    [Route("api/pitchfinder/store")]
    [Authorize]
    public class StoreController : ControllerBase
    {
        private readonly StoreService _storeService;

        public StoreController(StoreService storeService)
        {
            _storeService = storeService;
        }

        [HttpPut("{id:int}")]
        public async Task<StoreUpdateResponse> UpdateStoreInfo([FromRoute] int id, [FromBody] StoreUpdateRequest request)
        {
            return await _storeService.UpdateStoreInfoAsync(id, request);
        }

        [HttpGet("{id:int}/pitchs")]
        public async Task<List<PitchItemResponse>> GetPitchs([FromRoute] int id)
        {
            return await _storeService.GetPitchsAsync(id);
        }
    }
}
