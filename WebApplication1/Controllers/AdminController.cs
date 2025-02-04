using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Models.ViewModels;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public AdminController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public async Task<IActionResult> Dashboard()
        {

            var dashboard = new AdminDashboardViewModel
            {
                //TODO create Admin service
                TotalBooks = await _context.Books.CountAsync(),
                DeletedBooks=await _context.Books.CountAsync(x=>x.IsDeleted),
                TotalUsers=await _userManager.Users.CountAsync(x=>x.UserName!= "admin@library.com"), //TODO: check with role instead of username
              //  DeletedUsers=await _userManager.Users
                TotalRentals=await _context.Rentals.CountAsync(),
                ActiveRentals=await _context.Rentals.CountAsync(x=>!x.IsReturned),
                RecentRentals=await _context.Rentals
                .Include(x => x.User)
                .Include(x => x.Book)
                .OrderByDescending(x=>x.RentalDate)
                .Where(x => !x.IsReturned)
                .ToListAsync()

            };

            return View(dashboard);

        }


        public async Task<ActionResult> Rentals()
        {

            var rentals = await _context.Rentals
                .Include(x => x.User)
                .Include(x => x.Book)
                .OrderByDescending(x=>!x.IsReturned)
                .OrderByDescending(x=>x.RentalDate)
                .ToListAsync();


            return View(rentals);
        }

        public async Task<ActionResult> ActiveRentals()
        {

            var rentals = await _context.Rentals
                .Include(x=>x.User)
                .Include(x=>x.Book)
                .Where(x=>!x.IsReturned)
                .ToListAsync();


            return View(rentals);
        }


        public async Task<ActionResult> Users(string? nameFilter, string? emailFilter, bool includeInactive = false)
        {
            var query = _userManager.Users
            .OrderByDescending(u => u.IsActive)
            .AsQueryable();

            if (!includeInactive)
            {
                query = query.Where(u => u.IsActive);
            }

            if (!string.IsNullOrEmpty(nameFilter))
            { //TODO merge firstName and lastName to make it easier to search
                query = query.Where(u => 
                    EF.Functions.Like(u.FirstName.ToLower(), $"%{nameFilter.ToLower()}%") ||
                    EF.Functions.Like(u.LastName.ToLower(), $"%{nameFilter.ToLower()}%")
                );
            }

            if (!string.IsNullOrEmpty(emailFilter))
            {
                query = query.Where(u => EF.Functions.Like(u.Email.ToLower(), $"%{emailFilter.ToLower()}%"));
            }

            ViewBag.NameFilter = nameFilter;
            ViewBag.EmailFilter = emailFilter;
            ViewBag.IncludeInactive = includeInactive;

            var users = await query.ToListAsync();
            return View(users);
        }


        public async Task<ActionResult> UpdateUser(string id)

        {
            var user = await _userManager.FindByIdAsync(id);

            if(user == null)
            {
                return NotFound();
            }   
            return View(user);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateUser(AppUser user)
        {

            if (!ModelState.IsValid)
            {
                 return View(user);
            }

            var existingUser = await _userManager.FindByIdAsync(user.Id); //TODO create admin service to seperate business layer

            if (existingUser == null)
            {
                return NotFound();
            }

            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Email = user.Email;
            existingUser.PhoneNumber = user.PhoneNumber;
            //existingUser.Role=user.Role // find how to change the roles and assign
            existingUser.IsActive = user.IsActive; // TODO add isVerified later

            var updatedUser = await _userManager.UpdateAsync(existingUser);

            return RedirectToAction(nameof(Users));
        }

    }

}