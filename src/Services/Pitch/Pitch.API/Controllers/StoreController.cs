﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pitch.API.Services;
using Pitch.API.ViewModels.Store.Requests;
using Pitch.API.ViewModels.Store.Responses;
using Shared.API.Identity;

namespace Pitch.API.Controllers
{
    [Route("api/pitchfinder/store")]
    [Authorize(PolicyNames.OWNER_API)]
    public class StoreController : ControllerBase
    {
        private readonly StoreService _storeService;

        public StoreController(StoreService storeService)
        {
            _storeService = storeService;
        }

        [HttpGet()]
        public async Task<StoreDetailResponse> GetStore()
        {
            return await _storeService.GetStoreAsync();
        }

        [HttpPut("{id:int}")]
        public async Task<EditStoreResponse> EditStoreInfo([FromRoute] int id, [FromForm] EditStoreRequest request)
        {
            return await _storeService.EditStoreInfoAsync(id, request);
        }

        [HttpGet("{id:int}/pitchs")]
        public async Task<List<PitchItemResponse>> GetPitchs([FromRoute] int id)
        {
            return await _storeService.GetPitchsAsync(id);
        }

        [HttpPost("{id:int}/pitchs")]
        public async Task AddPitch([FromRoute] int id, [FromBody] AddPitchRequest request)
        {
            await _storeService.AddPitchAsync(id, request);
        }

        [HttpPut("pitchs/{id:int}")]
        public async Task EditPitchInfo([FromRoute] int id, [FromBody] EditPitchRequest request)
        {
            await _storeService.EditPitchInfoAsync(id, request);
        }
    }
}
