using Order.Domain.Enums;

namespace Order.API.ViewModels.Order.Responses
{
    public class OrderHistoryItemReponse
    {
        public int OrderId { get; set; }
        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public int PitchId { get; set; }
        public string PitchName { get; set; }
        public string Address { get; set; }
        public int Price { get; set; }
        public OrderStatusEnum Status { get; set; }
        public string Note { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
