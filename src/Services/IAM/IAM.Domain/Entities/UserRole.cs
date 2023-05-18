using Microsoft.AspNetCore.Identity;

namespace IAM.Domain.Entities
{
    public class UserRole : IdentityUserRole<int>
    {
        public UserRole()
        {

        }

        public virtual Role Role { get; private set; }
        public virtual User User { get; private set; }
    }
}
