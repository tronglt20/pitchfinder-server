using Shared.Domain.Entities;

namespace Pitch.Domain.Entities
{
    public class StoreRating : BaseEntity<int>
    {
        public StoreRating()
        {

        }

        public int Rating { get; set; }
        public int StoreId { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedOn { get; set; }

        public virtual Store Store { get; set; }
        public virtual User User{ get; set; }
    }
}
