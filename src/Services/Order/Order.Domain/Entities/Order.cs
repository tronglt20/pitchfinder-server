using Order.Domain.Enums;
using Shared.Domain.Entities;

namespace Order.Domain.Entities
{
    public class Order : BaseEntity<int>
    {
        public int StoreId { get; set; }
        public int PitchId { get; set; }
        public int PitchType { get; set; }
        public OrderStatusEnum Status { get; set; }
        public int Price { get; set; }
        public string Note { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedById { get; set; }

        public virtual User CreatedBy { get; set; }
    }
}
