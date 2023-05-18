﻿using Microsoft.AspNetCore.Identity;

namespace IAM.Domain.Entities
{
    public class User : IdentityUser<int>
    {
        public User()
        {

        }

        public User(string name, string email)
        {
            UserName = name;
            NormalizedUserName = name;
            Email = email;
            NormalizedEmail = email;
        }

        public string Address { get; set; }
        public int AvatarId { get; set; }

        public virtual Attachment Avatar { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; private set; } = new HashSet<UserRole>();
    }
}
