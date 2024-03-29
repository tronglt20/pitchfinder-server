﻿using Microsoft.AspNetCore.Identity;

namespace IAM.Domain.Entities
{
    public class User : IdentityUser<int>
    {
        public User()
        {

        }

        public User(string email, Role role)
        {
            UserName = email;
            NormalizedUserName = email;
            Email = email;
            NormalizedEmail = email;
            UserRoles.Add(new UserRole(this, role));
        }

        public string Address { get; set; }
        public int? AvatarId { get; set; }

        public virtual Attachment Avatar { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();
    }
}
