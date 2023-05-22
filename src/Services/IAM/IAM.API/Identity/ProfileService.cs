using IAM.Domain.Entities;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Shared.Infrastructure.DTOs;
using System.Security.Claims;

namespace IAM.API.Identity
{
    public class ProfileService : IProfileService
    {
        protected UserManager<User> _userManager;

        public ProfileService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var user = await _userManager.GetUserAsync(context.Subject);
            var roleId = user.UserRoles.Select(_ => _.RoleId).FirstOrDefault();
            var claims = new List<Claim>
            {
                new Claim(AppClaimType.UserId, user.Id.ToString()),
                new Claim(AppClaimType.UserName, user.UserName),
                new Claim(AppClaimType.UserEmail, user.Email),
                new Claim(AppClaimType.RoleId, roleId.ToString()),
            };

            context.IssuedClaims.AddRange(claims);
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var user = await _userManager.GetUserAsync(context.Subject);
            context.IsActive = user != null;
        }
    }
}
