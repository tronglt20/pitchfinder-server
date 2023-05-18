using Microsoft.AspNetCore.Identity;

namespace Pitch.Domain.Entities
{
    public class User : IdentityUser<int>
    {
        public User()
        {
            
        }

        public virtual ICollection<Store> Stores { get; set; } = new HashSet<Store>();
    }
}
