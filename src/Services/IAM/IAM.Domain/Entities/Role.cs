using Microsoft.AspNetCore.Identity;

namespace IAM.Domain.Entities
{
    public class Role : IdentityRole<int>
    {
        public Role()
        {

        }

        public virtual ICollection<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();
    }
}
