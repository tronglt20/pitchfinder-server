using Order.API.ViewModels.Order.Requests;
using Order.API.ViewModels.Order.Responses;
using Shared.API.ViewModels;
using Shared.Domain.Interfaces;
using Shared.Infrastructure.DTOs;

namespace Order.API.Services
{
    public class OrderService
    {
        private readonly IDistributedCacheRepository _distributedCacheRepo;
        private readonly IUserInfo _userInfo;

        public async Task<OrderConfirmationResponse> OrderAsync(int storeId, OrderConfirmationRequest request)
        {
            var filteringRequest = await _distributedCacheRepo.GetAsync<PitchFilteringRequest>($"filtering-request-{_userInfo.Id}");

            return new OrderConfirmationResponse();
        }
    }
}
