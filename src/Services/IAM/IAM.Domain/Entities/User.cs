using Microsoft.AspNetCore.Identity;

namespace IAM.Domain.Entities
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
    }
}
