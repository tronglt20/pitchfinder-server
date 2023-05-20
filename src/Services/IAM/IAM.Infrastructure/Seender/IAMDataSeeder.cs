using IAM.Domain.Entities;
using Shared.Domain.Enums;

namespace IAM.Infrastructure.Seender
{
    public class IAMDataSeeder
    {
        public static async Task SeedAsync(IAMDbContext context)
        {
            await SeendRoleAsync(context);
        }

        private static async Task SeendRoleAsync(IAMDbContext context)
        {
            if (!context.Roles.Any())
            {
                var roles = new List<Role>
                {
                    new Role
                    {
                        Id = (int)RoleEnum.Owner,
                        Name = RoleEnum.Owner.ToString(),
                        NormalizedName = RoleEnum.Owner.ToString(),
                    },
                    new Role
                    {
                        Id = (int)RoleEnum.Customer,
                        Name = RoleEnum.Customer.ToString(),
                        NormalizedName = RoleEnum.Customer.ToString(),
                    }
                };

                context.AddRange(roles);
                context.SaveChanges();
            }
        }
    }
}
