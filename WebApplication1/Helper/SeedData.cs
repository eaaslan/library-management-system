//using Microsoft.AspNetCore.Identity;
//using WebApplication1.Models;

//public static class SeedData
//{
//    public static async Task Initialize(IServiceProvider serviceProvider)
//    {
//        var roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();
//        var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

//        string[] roleNames = { "Admin", "User" };
//        foreach (var roleName in roleNames)
//        {
//            var roleExist = await roleManager.RoleExistsAsync(roleName);
//            if (!roleExist)
//            {
//                await roleManager.CreateAsync(new AppRole { Name = roleName });
//            }
//        }

//        // create admin user
//        var adminUser = new AppUser
//        {
//            UserName = "admin@example.com",
//            Email = "admin@example.com"
//        };


//        string adminPassword = "Admin123!";



//        // create admin user
//        var casualUser = new AppUser
//        {
//            UserName = "user@example.com",
//            Email = "user@example.com"
//        };

//        string userPassword = "User123!";



//        var user = await userManager.FindByEmailAsync(adminUser.Email);
//        if (user == null)
//        {
//            var createAdmin = await userManager.CreateAsync(adminUser, adminPassword);
//            if (createAdmin.Succeeded)
//            {
//                await userManager.AddToRoleAsync(adminUser, "Admin");
//            }
//        }

//        var userNormal = await userManager.FindByEmailAsync(casualUser.Email);
//        if (userNormal == null)
//        {
//            var createUser = await userManager.CreateAsync(casualUser, userPassword);
//            if (createUser.Succeeded)
//            {
//                await userManager.AddToRoleAsync(casualUser, "User");
//            }
//        }
//    }
//}