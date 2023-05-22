using Microsoft.AspNetCore.Identity;

namespace Pitch.Domain.Entities
{
    public class User : IdentityUser<int>
    {
        public User()
        {

        }

        public User(int id, string email)
        {
            Id = id;
            UserName = email;
            NormalizedUserName = email;
            Email = email;
            NormalizedEmail = email;
        }

        public virtual ICollection<Store> Stores { get; set; } = new HashSet<Store>();
        public virtual ICollection<StoreRating> StoreRatings { get; set; } = new HashSet<StoreRating>();
        public virtual ICollection<StoreComment> StoreComments { get; set; } = new HashSet<StoreComment>();

    }
}
