using Shared.Domain.Entities;

namespace Order.Domain.Entities
{
    public class Order : BaseEntity<int>
    {

        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
    }
}
