using Microsoft.AspNetCore.Identity;

namespace Order.Domain.Entities
{
    public class User : IdentityUser<int>
    {
        public User()
        {

        }
    }
}
