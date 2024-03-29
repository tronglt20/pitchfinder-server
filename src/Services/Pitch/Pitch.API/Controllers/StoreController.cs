﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pitch.API.Services;
using Pitch.API.ViewModels.Store.Requests;
using Pitch.API.ViewModels.Store.Responses;
using Pitch.Domain.Enums;
using Shared.API.Identity;

namespace Pitch.API.Controllers
{
    [Route("api/pitch/store")]
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
            return await _storeService.GetStoreDetailAsync();
        }

        [HttpPut()]
        public async Task<EditStoreResponse> EditStoreInfo([FromForm] EditStoreRequest request)
        {
            return await _storeService.EditStoreInfoAsync(request);
        }

        [HttpGet("pitchs")]
        public async Task<List<PitchItemResponse>> GetPitchs([FromQuery] string keyName, [FromQuery] PitchTypeEnum? pitchType)
        {
            return await _storeService.GetPitchsAsync(keyName, pitchType);
        }

        [HttpPost("pitchs")]
        public async Task AddPitch([FromForm] AddPitchRequest request)
        {
            await _storeService.AddPitchAsync(request);
        }

        [HttpPut("pitchs/{id:int}")]
        public async Task EditPitchInfo([FromRoute] int id, [FromForm] EditPitchRequest request)
        {
            await _storeService.EditPitchInfoAsync(id, request);
        }
    }
}
