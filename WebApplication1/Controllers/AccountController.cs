using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication1.Models.DTOs;
using WebApplication1.Data;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication1.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public AccountController(
            UserManager<AppUser> userManager, 
            SignInManager<AppUser> signInManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }


        //[Authorize(Roles = "Admin")]
        //public async Task<IEnumerable<Book>> GetFilteredUsers(string? nameFilter, string? emailFilter)
        ////merge firstName and lastName to make it easier to search
        //{
        //    var query = _context.Books.AsQueryable();

        //    if (!string.IsNullOrEmpty(nameFilter))
        //    {
        //        query = query.Where(b => EF.Functions.Like(b.FirstName.ToLower(), $"%{FirstName.ToLower()}%"));
        //    }

        //    if (!string.IsNullOrEmpty(emailFilter) && emailFilter != "All")
        //    {
        //        query = query.Where(b => b.email == emailFilter);
        //    }

        //    return await query.ToListAsync();
        //}




        [HttpGet]
        public async Task<IActionResult> MyRentals()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var rentals = await _context.Rentals
                .Include(r => r.Book)
                .ThenInclude(b => b.Category)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.RentalDate)
                .ToListAsync();

            return View(rentals);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto user)
        {
            if (!ModelState.IsValid)
                return View(user);

            var result = await _signInManager.PasswordSignInAsync(user.Email, user.Password, false, false);
            if (result.Succeeded)
            {
                // Check if user is active
                var appUser = await _userManager.FindByEmailAsync(user.Email);
                if (!appUser.IsActive)
                {
                    await _signInManager.SignOutAsync();
                    ModelState.AddModelError(string.Empty, "This account has been deactivated. Please contact an administrator.");
                    return View(user);
                }

                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("Dashboard", "Admin");
                }
                return RedirectToAction("Index", "Book");
            }

            ModelState.AddModelError(string.Empty, "Invalid email or password. Please try again.");
            return View(user);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto userRegister)
        {
            if (!ModelState.IsValid)
                return View(userRegister);

            var user = new AppUser
            {
                UserName = userRegister.Email,
                Email = userRegister.Email,
                FirstName = userRegister.FirstName,
                LastName = userRegister.LastName,
                IsActive = true,      // User can login
                IsVerified = false    // But cannot rent books until verified
            };

            var result = await _userManager.CreateAsync(user, userRegister.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User"); // Add user to default "User" role
                await _signInManager.SignInAsync(user, isPersistent: false);

                // Create notification for admin
                var notification = new Notification
                {
                    Title = "New User Registration",
                    Message = $"New user {user.Email} has registered and needs verification.",
                    CreatedAt = DateTime.UtcNow,
                    IsRead = false,
                    Type = "UserRegistration",
                    UserId = user.Id
                };

                _context.Notifications.Add(notification);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Book");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(userRegister);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Book");
        }
    }
}
