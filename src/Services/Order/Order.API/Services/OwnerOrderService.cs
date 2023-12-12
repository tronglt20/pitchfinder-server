#nullable disable

using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Core.Operations;
using Order.API.ViewModels.Dashboard.Response;
using Order.API.ViewModels.Order.Responses;
using Order.Domain.Enums;
using Order.Domain.Interfaces;
using Order.Infrastructure;
using Pitch.Domain.Enums;
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

        public async Task<List<OrderHistoryItemReponse>> GetOrdersAsync(string keyName, int? pitchType, bool isDashboard)
        {
            var pichInfo = await _pitchGrpcService.GetOwnerPitchInfoAsync();
            var stores = pichInfo.Stores.FirstOrDefault();
            var pitchs = pichInfo.Pitchs.ToList();

            var orders = await _orderRepo.GetOwnerOrdersAsync(stores.StoreId, pitchType);
            if (orders == null)
                return null;

            if (isDashboard)
                orders = orders.OrderByDescending(_ => _.CreatedOn).Take(3).ToList(); ;

            var result = orders.Select(_ => new OrderHistoryItemReponse
            {
                OrderId = _.Id,
                PitchId = _.PitchId,
                PitchName = pitchs.Where(s => s.PitchId == _.PitchId).Select(s => s.PitchName).FirstOrDefault(),
                Price = _.Price,
                Status = _.Status,
                Note = _.Note,
                Date = _.Date,
                Start = _.Start,
                End = _.End,
                CreatedOn = _.CreatedOn,
                CreatedByName = _.CreatedBy.UserName,
                CreatedById = _.CreatedById
            }).ToList();

            return result.Where(_ => string.IsNullOrEmpty(keyName) ? true : _.PitchName.Contains(keyName)).ToList();
        }

        public async Task<List<CustomerItemReponse>> GetCustomersAsync(string keyname)
        {
            var pichInfo = await _pitchGrpcService.GetOwnerPitchInfoAsync();
            var stores = pichInfo.Stores.FirstOrDefault();

            return await _orderRepo.GetQuery(_ => _.StoreId == stores.StoreId && _.Status == OrderStatusEnum.Succesed)
                    .GroupBy(_ => _.CreatedById)
                    .Select(_ => new CustomerItemReponse()
                    {
                        Id = _.Key,
                        Name = _.Select(o => o.CreatedBy.UserName).FirstOrDefault(),
                        PhoneNumber  = _.Select(o => o.CreatedBy.PhoneNumber).FirstOrDefault(),
                        NumberOfOrder = _.Count()
                    })
                    .Where(_ => string.IsNullOrEmpty(keyname) ? true : _.Name.Contains(keyname))
                    .ToListAsync();
        }

        public async Task<List<PitchTypeDashboardModel>> GetPitchTypeDashboardAsync()
        {
            var pichInfo = await _pitchGrpcService.GetOwnerPitchInfoAsync();
            var store = pichInfo.Stores.FirstOrDefault();
            var pitchTypes = pichInfo.Pitchs.DistinctBy(_ => _.PitchType).Select(_ => _.PitchType).ToList();

            var result  = new List<PitchTypeDashboardModel>();

            foreach (var pitchType in pitchTypes)
            {
                var revenue = await _orderRepo.GetRevanueByPitchTypeAsync(store.StoreId, pitchType);
                result.Add(new PitchTypeDashboardModel()
                {
                    PitchType = (PitchTypeEnum)pitchType,
                    Revenue = revenue,
                });
            }

            return result;
        }

        public async Task<List<PitchNameDashboardModel>> GetPitchNameDashboardAsync()
        {
            var pichInfo = await _pitchGrpcService.GetOwnerPitchInfoAsync();
            var store = pichInfo.Stores.FirstOrDefault();
            var pitchs = pichInfo.Pitchs.ToList();

            var result = new List<PitchNameDashboardModel>();

            foreach (var pitch in pitchs)
            {
                var revenue = await _orderRepo.GetRevanueByPitchNameAsync(store.StoreId, pitch.PitchId);
                result.Add(new PitchNameDashboardModel()
                {
                    PitchName = pitch.PitchName,
                    Revenue = revenue,
                });
            }

            return result;
        }

        public async Task<List<RevanueByMonthModel>> RevanueByMonthModelAsync()
        {
            var result = new List<RevanueByMonthModel>();
            var year = DateTime.Now.Year;

            for (int month = 1; month <= 12; month++)
            {
                var startDate = new DateTime(year, month, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);

                var monthlyRevenue = await _orderRepo.GetRevanueByMonthAsync(startDate, endDate);

                result.Add(new RevanueByMonthModel
                {
                    Month = startDate.ToString("MMMM"),
                    Revenue = monthlyRevenue,
                });
            }

            return result;
        }
    }
}
