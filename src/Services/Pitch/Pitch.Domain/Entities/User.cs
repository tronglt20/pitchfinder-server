using Microsoft.AspNetCore.Identity;

namespace Pitch.Domain.Entities
{
    public class User : IdentityUser<int>
    {
        public User()
        {

        }

        public User(string email)
        {
            UserName = email;
            NormalizedUserName = email;
            Email = email;
            NormalizedEmail = email;
        }

        public virtual ICollection<Store> Stores { get; set; } = new HashSet<Store>();
    }
}
