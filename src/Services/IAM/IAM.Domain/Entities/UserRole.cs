using Microsoft.AspNetCore.Identity;

namespace IAM.Domain.Entities
{
    public class UserRole : IdentityUserRole<int>
    {
        public UserRole()
        {

        }

        public UserRole(User user, Role role)
        {
            User = user;
            Role = role;
        }

        public virtual Role Role { get; set; }
        public virtual User User { get; set; }
    }
}
