using ElevaniPaymentGateway.Core.Entities;
using ElevaniPaymentGateway.Core.Enums;
using ElevaniPaymentGateway.Core.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Text;

namespace ElevaniPaymentGateway.Persistence.DbInitializer
{
    public static class DbInitializer
    {
        private static string EmailAddress = "elevanilimited@gmail.com";
        private static string Password = "Password12345$";
        public static IEnumerable<Role> RolesList()
        {
            return new List<Role>()
            {
                new Role
                {
                    Id = Guid.NewGuid(),
                    Name = nameof(UserRoles.SuperAdmin),
                    Description ="Super admin role",
                    CreatedBy = "System"
                },
                new Role
                {
                    Id = Guid.NewGuid(),
                    Name = nameof(UserRoles.Admin),
                    Description = "Admin role",
                    CreatedBy = "System"
                },
                new Role
                {
                    Id = Guid.NewGuid(),
                    Name = nameof(UserRoles.Merchant),
                    Description = "Merchant role",
                    CreatedBy = "System"
                },
            };
        }

        public static async Task SeedRoles(this IHost host)
        {
            var serviceProvider = host.Services.CreateScope().ServiceProvider;
            var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();

            foreach (var role in RolesList())
            {
                var existingRole = await roleManager.FindByNameAsync(role.Name);
                if (existingRole is null)
                {
                    var result = await roleManager.CreateAsync(role);
                    if (!result.Succeeded) throw new GenericException("An error occurred while trying to seed application roles");
                }
            }
        }

        public static async Task SeedSuperAdmin(this IHost host)
        {
            var serviceProvider = host.Services.CreateScope().ServiceProvider;
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();

            var user = await userManager.FindByEmailAsync(EmailAddress);
            if (user is null)
            {
                var superAdmin = new User
                {
                    FirstName = "Elevani",
                    LastName = "Elevani",
                    UserName = EmailAddress,
                    Email = EmailAddress,
                    PhoneNumber = "1234567890",
                    Status = UserStatus.Active,
                    EmailConfirmed = true,
                    DateOfBirth = DateTime.Now,
                    CreatedBy = "System"
                };

                var userResult = await userManager.CreateAsync(superAdmin, Password);
                if (!userResult.Succeeded)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var error in userResult.Errors)
                    {
                        sb.AppendLine(error.Description);
                    }
                    Log.Information($"An error occurred while trying to seed the super admin user >>> {sb.ToString()}");
                    throw new GenericException("An error occurred while trying to seed the super admin user", sb.ToString());
                }
                else
                {
                    var userRoleResult = await userManager.AddToRoleAsync(superAdmin, nameof(UserRoles.SuperAdmin));
                    if (!userRoleResult.Succeeded) throw new GenericException("An error occurred while trying to seed the super admin user");
                }
            }
        }
    }
}
