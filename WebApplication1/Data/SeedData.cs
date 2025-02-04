//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using WebApplication1.Models;

//namespace WebApplication1.Data
//{
//    public static class SeedData
//    {
//        public static async Task Initialize(IServiceProvider serviceProvider)
//        {
//            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
//            var roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();
//            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

//            // Create roles
//            string[] roleNames = { "Admin", "User", "LibraryWorker" };
//            foreach (var roleName in roleNames)
//            {
//                var roleExist = await roleManager.RoleExistsAsync(roleName);
//                if (!roleExist)
//                {
//                    await roleManager.CreateAsync(new AppRole { Name = roleName });
//                }
//            }

//            // Create admin user
//            var adminEmail = "admin@library.com";
//            var adminUser = await userManager.FindByEmailAsync(adminEmail);

//            if (adminUser == null)
//            {
//                var admin = new AppUser
//                {
//                    UserName = adminEmail,
//                    Email = adminEmail,
//                    FirstName = "Admin",
//                    LastName = "User",
//                    EmailConfirmed = true,
//                    IsActive = true
//                };

//                var result = await userManager.CreateAsync(admin, "Admin123!");
//                if (result.Succeeded)
//                {
//                    await userManager.AddToRoleAsync(admin, "Admin");
//                }
//            }

//            // Create library workers if they don't exist
//            var libraryWorkers = new[]
//            {
//                new { Email = "worker1@library.com", FirstName = "Sarah", LastName = "Johnson" },
//                new { Email = "worker2@library.com", FirstName = "Mike", LastName = "Wilson" },
//                new { Email = "worker3@library.com", FirstName = "Emily", LastName = "Brown" }
//            };

//            foreach (var worker in libraryWorkers)
//            {
//                var existingWorker = await userManager.FindByEmailAsync(worker.Email);
//                if (existingWorker == null)
//                {
//                    var newWorker = new AppUser
//                    {
//                        UserName = worker.Email,
//                        Email = worker.Email,
//                        FirstName = worker.FirstName,
//                        LastName = worker.LastName,
//                        EmailConfirmed = true,
//                        IsActive = true
//                    };

//                    var result = await userManager.CreateAsync(newWorker, "Worker123!");
//                    if (result.Succeeded)
//                    {
//                        await userManager.AddToRoleAsync(newWorker, "LibraryWorker");
//                    }
//                }
//            }

//            // Create sample users if they don't exist
//            var sampleUsers = new[]
//            {
//                new { Email = "user1@test.com", FirstName = "John", LastName = "Doe", IsActive = true },
//                new { Email = "user2@test.com", FirstName = "Jane", LastName = "Smith", IsActive = true },
//                new { Email = "user3@test.com", FirstName = "Robert", LastName = "Johnson", IsActive = false },
//                new { Email = "user4@test.com", FirstName = "Mary", LastName = "Williams", IsActive = true },
//                new { Email = "user5@test.com", FirstName = "James", LastName = "Brown", IsActive = false },
//                new { Email = "user6@test.com", FirstName = "Patricia", LastName = "Davis", IsActive = true },
//                new { Email = "user7@test.com", FirstName = "Michael", LastName = "Miller", IsActive = true },
//                new { Email = "user8@test.com", FirstName = "Linda", LastName = "Wilson", IsActive = false },
//                new { Email = "user9@test.com", FirstName = "William", LastName = "Moore", IsActive = true },
//                new { Email = "user10@test.com", FirstName = "Elizabeth", LastName = "Taylor", IsActive = true },
//                new { Email = "user11@test.com", FirstName = "David", LastName = "Anderson", IsActive = false },
//                new { Email = "user12@test.com", FirstName = "Jennifer", LastName = "Thomas", IsActive = true },
//                new { Email = "user13@test.com", FirstName = "Richard", LastName = "Jackson", IsActive = true },
//                new { Email = "user14@test.com", FirstName = "Barbara", LastName = "White", IsActive = false },
//                new { Email = "user15@test.com", FirstName = "Joseph", LastName = "Harris", IsActive = true },
//                new { Email = "user16@test.com", FirstName = "Susan", LastName = "Martin", IsActive = true },
//                new { Email = "user17@test.com", FirstName = "Thomas", LastName = "Thompson", IsActive = false },
//                new { Email = "user18@test.com", FirstName = "Margaret", LastName = "Garcia", IsActive = true },
//                new { Email = "user19@test.com", FirstName = "Charles", LastName = "Martinez", IsActive = true },
//                new { Email = "user20@test.com", FirstName = "Sandra", LastName = "Robinson", IsActive = false }
//            };

//            foreach (var sampleUser in sampleUsers)
//            {
//                var existingUser = await userManager.FindByEmailAsync(sampleUser.Email);
//                if (existingUser == null)
//                {
//                    var user = new AppUser
//                    {
//                        UserName = sampleUser.Email,
//                        Email = sampleUser.Email,
//                        FirstName = sampleUser.FirstName,
//                        LastName = sampleUser.LastName,
//                        EmailConfirmed = true,
//                        IsActive = sampleUser.IsActive
//                    };

//                    var result = await userManager.CreateAsync(user, "User123!");
//                    if (result.Succeeded)
//                    {
//                        await userManager.AddToRoleAsync(user, "User");
//                    }
//                }
//            }

//            // Create sample rental records if none exist
//            if (!context.Rentals.Any())
//            {
//                var books = await context.Books.ToListAsync();
//                var users = await userManager.Users.Where(u => u.IsActive).ToListAsync();

//                if (books.Any() && users.Any())
//                {
//                    var random = new Random();

//                    // Create past returned rentals
//                    for (int i = 0; i < 30; i++)
//                    {
//                        var book = books[random.Next(books.Count)];
//                        var user = users[random.Next(users.Count)];
//                        var rentalDate = DateTime.UtcNow.AddDays(-random.Next(30, 90));
//                        var returnDate = rentalDate.AddDays(random.Next(1, 14));

//                        context.Rentals.Add(new Rental
//                        {
//                            BookId = book.Id,
//                            ISBN = book.ISBN,
//                            UserId = user.Id,
//                            RentalDate = rentalDate,
//                            ReturnDate = returnDate,
//                            IsReturned = true
//                        });
//                    }

//                    // Create active rentals
//                    for (int i = 0; i < 10; i++)
//                    {
//                        var book = books[random.Next(books.Count)];
//                        var user = users[random.Next(users.Count)];
//                        var rentalDate = DateTime.UtcNow.AddDays(-random.Next(1, 13));

//                        book.Available = false;
//                        context.Rentals.Add(new Rental
//                        {
//                            BookId = book.Id,
//                            ISBN = book.ISBN,
//                            UserId = user.Id,
//                            RentalDate = rentalDate,
//                            ReturnDate = rentalDate.AddDays(14),
//                            IsReturned = false
//                        });
//                    }

//                    // Create overdue rentals
//                    for (int i = 0; i < 5; i++)
//                    {
//                        var book = books[random.Next(books.Count)];
//                        var user = users[random.Next(users.Count)];
//                        var rentalDate = DateTime.UtcNow.AddDays(-random.Next(15, 30));

//                        book.Available = false;
//                        context.Rentals.Add(new Rental
//                        {
//                            BookId = book.Id,
//                            ISBN = book.ISBN,
//                            UserId = user.Id,
//                            RentalDate = rentalDate,
//                            ReturnDate = rentalDate.AddDays(14),
//                            IsReturned = false
//                        });
//                    }

//                    await context.SaveChangesAsync();
//                }
//            }
//        }
//    }
//} 