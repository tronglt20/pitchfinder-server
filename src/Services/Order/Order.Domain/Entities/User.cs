using Microsoft.AspNetCore.Identity;

namespace Order.Domain.Entities
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

        public virtual ICollection<Order> Orders { get; set; } = new HashSet<Order>();

    }
}
