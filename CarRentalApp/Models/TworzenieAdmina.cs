using CarRentalApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;

namespace CarRentalApp.Models
{
    public class TworzenieAdmina
    {
        public static async Task InicjalizujRole(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roleNames = { "Admin", "User" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            if (userManager.Users.All(u => u.UserName != "admin@admin.pl"))
            {
                var adminUser = new ApplicationUser
                {
                    UserName = "admin@admin.pl",
                    Email = "admin@admin.pl",
                    FirstName = "Admin",
                    LastName = "Systemu",
                    EmailConfirmed = true,
                    IsAdmin = true,
                };

                await userManager.CreateAsync(adminUser, "!Admin123");
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
        public static async Task InicjalizujKategorie(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<AppDbContext>();

            // Sprawdzamy czy tabela jest pusta
            if (!context.CarCategories.Any())
            {
                context.CarCategories.AddRange(
                    new CarCategory { Name = "Sedan" },
                    new CarCategory { Name = "SUV" },
                    new CarCategory { Name = "Kombi" },
                    new CarCategory { Name = "Hatchback" },
                    new CarCategory { Name = "Sportowy" }
                );

                await context.SaveChangesAsync();
            }
        }
    }
}
