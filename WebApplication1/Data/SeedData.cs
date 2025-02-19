using CsvHelper;
using System.Globalization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public static class SeedData
    {
        private static async Task ImportCategoriesFromCsv(ApplicationDbContext context)
        {
            if (!context.Categories.Any())
            {
                using var reader = new StreamReader("Data/CSV/categories.csv");
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                var categories = csv.GetRecords<Category>().ToList();
                await context.Categories.AddRangeAsync(categories);
                await context.SaveChangesAsync();
            }
        }

        private static async Task ImportBooksFromCsv(ApplicationDbContext context)
        {
            if (!context.Books.Any())
            {
                using var reader = new StreamReader("Data/CSV/books.csv");
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                var books = csv.GetRecords<Book>().ToList();
                await context.Books.AddRangeAsync(books);
                await context.SaveChangesAsync();
            }
        }

        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

            // Ensure database is created and migrated
            await context.Database.MigrateAsync();

            // 1. Import Categories first
            await ImportCategoriesFromCsv(context);

            // 2. Import Books second (since they might depend on categories)
            await ImportBooksFromCsv(context);

            // 3. Create roles if they don't exist
            string[] roleNames = { "Admin", "LibraryStaff", "User" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new AppRole { Name = roleName });
                }
            }

            // 4. Create admin user
            var adminEmail = "admin@example.com";
            var admin = await userManager.FindByEmailAsync(adminEmail);
            if (admin == null)
            {
                admin = new AppUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Admin",
                    LastName = "User",
                    EmailConfirmed = true,
                    IsActive = true,
                    IsVerified = true
                };
                var result = await userManager.CreateAsync(admin, "Admin123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }

            // Create 3 library staff users
            for (int i = 1; i <= 3; i++)
            {
                var staffEmail = $"staff{i}@example.com";
                var staff = await userManager.FindByEmailAsync(staffEmail);
                if (staff == null)
                {
                    staff = new AppUser
                    {
                        UserName = staffEmail,
                        Email = staffEmail,
                        FirstName = $"Staff{i}",
                        LastName = "User",
                        EmailConfirmed = true,
                        IsActive = true,
                        IsVerified = true
                    };
                    var result = await userManager.CreateAsync(staff, "Staff123!");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(staff, "LibraryStaff");
                    }
                }
            }

            // Create 10 regular users (5 active and verified)
            for (int i = 1; i <= 10; i++)
            {
                var userEmail = $"user{i}@example.com";
                var user = await userManager.FindByEmailAsync(userEmail);
                if (user == null)
                {
                    user = new AppUser
                    {
                        UserName = userEmail,
                        Email = userEmail,
                        FirstName = $"User{i}",
                        LastName = "Test",
                        EmailConfirmed = true,
                        IsActive = i <= 5, // First 5 users are active
                        IsVerified = i <= 5 // First 5 users are verified
                    };
                    var result = await userManager.CreateAsync(user, "User123!");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "User");
                    }
                }
            }

            // Create sample rental records if none exist
            if (!context.Rentals.Any() && context.Books.Any())
            {
                var books = await context.Books.ToListAsync();
                var activeUsers = await userManager.GetUsersInRoleAsync("User");
                activeUsers = activeUsers.Where(u => u.IsActive && u.IsVerified).ToList();

                if (books.Any() && activeUsers.Any())
                {
                    var random = new Random();

                    // Create 10 returned rentals
                    for (int i = 0; i < 10; i++)
                    {
                        var book = books[random.Next(books.Count)];
                        var user = activeUsers[random.Next(activeUsers.Count)];
                        var rental = new Rental
                        {
                            BookId = book.Id,
                            ISBN = book.ISBN,
                            UserId = user.Id,
                            IsReturned = true
                        };
                        
                        rental.SetRentalDate(DateTime.UtcNow.AddDays(-random.Next(15, 30)));
                        rental.SetReturnDate(rental.RentalDate.AddDays(random.Next(1, 13)));
                        
                        context.Rentals.Add(rental);
                    }

                    // Create 3 active rentals
                    for (int i = 0; i < 3; i++)
                    {
                        var book = books[random.Next(books.Count)];
                        var user = activeUsers[random.Next(activeUsers.Count)];
                        var rental = new Rental
                        {
                            BookId = book.Id,
                            ISBN = book.ISBN,
                            UserId = user.Id,
                            IsReturned = false
                        };
                        
                        rental.SetRentalDate(DateTime.UtcNow.AddDays(-random.Next(1, 10)));
                        
                        book.Available = false;
                        context.Rentals.Add(rental);
                    }

                    await context.SaveChangesAsync();
                }
            }
        }
    }
} 