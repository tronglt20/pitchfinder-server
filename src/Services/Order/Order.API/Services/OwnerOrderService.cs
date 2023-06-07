#nullable disable

using Microsoft.EntityFrameworkCore;
using Order.API.ViewModels.Order.Responses;
using Order.Domain.Enums;
using Order.Domain.Interfaces;
using Order.Infrastructure;
using Shared.Domain.Interfaces;
using Shared.Infrastructure.DTOs;

namespace Order.API.Services
{
    public class OwnerOrderService
    {
        private readonly IDistributedCacheRepository _distributedCacheRepo;
        private readonly PitchGrpcService _pitchGrpcService;
        private readonly IUnitOfWorkBase _unitOfWorkBase;
        private readonly IOrderRepository _orderRepo;
        private readonly IUserInfo _userInfo;

        public OwnerOrderService(IOrderRepository orderRepo
          , IDistributedCacheRepository distributedCacheRepo
          , IUserInfo userInfo
          , PitchGrpcService pitchGrpcService
          , IUnitOfWorkBase<OrderDbContext> unitOfWorkBase)
        {
            _orderRepo = orderRepo;
            _distributedCacheRepo = distributedCacheRepo;
            _userInfo = userInfo;
            _pitchGrpcService = pitchGrpcService;
            _unitOfWorkBase = unitOfWorkBase;
        }

        public async Task<List<OrderHistoryItemReponse>> GetOrdersAsync()
        {
            var pichInfo = await _pitchGrpcService.GetOwnerPitchInfoAsync();
            var stores = pichInfo.Stores.FirstOrDefault();
            var pitchs = pichInfo.Pitchs.ToList();

            var orders = await _orderRepo.GetOwnerOrdersAsync(stores.StoreId);
            if (orders == null)
                return null;

            return orders.Select(_ => new OrderHistoryItemReponse
            {
                OrderId = _.Id,
                StoreId = _.StoreId,
                StoreName = stores.StoreName,
                Address = stores.Address,
                PitchId = _.PitchId,
                PitchName = pitchs.Where(s => s.PitchId == _.PitchId).Select(s => s.PitchName)
                                  .FirstOrDefault(),
                Price = _.Price,
                Status = _.Status,
                Note = _.Note,
                Date = _.Date,
                Start = _.Start,
                End = _.End,
                CreatedOn = _.CreatedOn,
            }).ToList();
        }

        public async Task<List<CustomerItemReponse>> GetCustomersAsync()
        {
            var pichInfo = await _pitchGrpcService.GetOwnerPitchInfoAsync();
            var stores = pichInfo.Stores.FirstOrDefault();

            return await _orderRepo.GetQuery(_ => _.StoreId == stores.StoreId && _.Status == OrderStatusEnum.Succesed)
                .GroupBy(_ => _.CreatedBy)
                .Select(_ => new CustomerItemReponse()
                {
                    Id = _.Key.Id,
                    Name = _.Key.UserName,
                    PhoneNumber = _.Key.PhoneNumber,
                    NumberOfOrder = _.Count()
                }).ToListAsync();
        }
    }
}
