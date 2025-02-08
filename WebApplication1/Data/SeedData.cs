using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            // Create sample rental records if none exist
            if (!context.Rentals.Any())
            {
                var books = await context.Books.ToListAsync();
                var users = await context.Users.Where(u => u.IsActive).ToListAsync();

                if (books.Any() && users.Any())
                {
                    var random = new Random();

                    // Create 20 returned rentals (normal returns)
                    for (int i = 0; i < 20; i++)
                    {
                        var book = books[random.Next(books.Count)];
                        var user = users[random.Next(users.Count)];
                        var rental = new Rental
                        {
                            BookId = book.Id,
                            ISBN = book.ISBN,
                            UserId = user.Id,
                            IsReturned = true
                        };
                        
                        // Set rental date between 15-60 days ago
                        rental.SetRentalDate(DateTime.UtcNow.AddDays(-random.Next(15, 60)));
                        // Set return date before due date
                        rental.SetReturnDate(rental.RentalDate.AddDays(random.Next(1, 13)));
                        
                        context.Rentals.Add(rental);
                    }

                    // Create 2 returned overdue rentals
                    for (int i = 0; i < 2; i++)
                    {
                        var book = books[random.Next(books.Count)];
                        var user = users[random.Next(users.Count)];
                        var rental = new Rental
                        {
                            BookId = book.Id,
                            ISBN = book.ISBN,
                            UserId = user.Id,
                            IsReturned = true
                        };
                        
                        // Set rental date 30-45 days ago
                        rental.SetRentalDate(DateTime.UtcNow.AddDays(-random.Next(30, 45)));
                        // Set return date after due date (overdue)
                        rental.SetReturnDate(rental.DueDate.AddDays(random.Next(1, 10)));
                        
                        context.Rentals.Add(rental);
                    }

                    // Create 3 active overdue rentals (not returned)
                    for (int i = 0; i < 3; i++)
                    {
                        var book = books[random.Next(books.Count)];
                        var user = users[random.Next(users.Count)];
                        var rental = new Rental
                        {
                            BookId = book.Id,
                            ISBN = book.ISBN,
                            UserId = user.Id,
                            IsReturned = false
                        };
                        
                        // Set rental date 20-30 days ago (will be overdue)
                        rental.SetRentalDate(DateTime.UtcNow.AddDays(-random.Next(20, 30)));
                        
                        book.Available = false;
                        context.Rentals.Add(rental);
                    }

                    // Create 5 active rentals (not overdue)
                    for (int i = 0; i < 5; i++)
                    {
                        var book = books[random.Next(books.Count)];
                        var user = users[random.Next(users.Count)];
                        var rental = new Rental
                        {
                            BookId = book.Id,
                            ISBN = book.ISBN,
                            UserId = user.Id,
                            IsReturned = false
                        };
                        
                        // Set rental date 1-10 days ago (will not be overdue)
                        rental.SetRentalDate(DateTime.UtcNow.AddDays(-random.Next(1, 10)));
                        
                        book.Available = false;
                        context.Rentals.Add(rental);
                    }

                    await context.SaveChangesAsync();
                }
            }

            // Update admin user handling
            var adminEmail = "admin@example.com";
            var admin = new AppUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FirstName = "Admin",
                LastName = "User",
                EmailConfirmed = true,
                IsActive = true,
                IsVerified = true  // Ensure admin is verified
            };
        }
    }
} 