using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Models.ViewModels;

namespace WebApplication1.Controllers
{
    //TODO make pagination lowercase
    [Authorize(Roles = "Admin")]
    public class AdminController : AdminBaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public AdminController(ApplicationDbContext context, UserManager<AppUser> userManager)
            : base(context)
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


        public async Task<IActionResult> Rentals(
            string sortOrder,
            string searchString,
            string statusFilter,
            int pageNumber = 1)
        {
            var pageSize = 10; // You can adjust this number
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentSort"] = sortOrder;
            ViewBag.CurrentStatus = statusFilter;

            var rentals = _context.Rentals
                .Include(x => x.User)
                .Include(x => x.Book)
                .AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(searchString))
            {
                rentals = rentals.Where(r => 
                    r.User.Email.Contains(searchString) ||
                    r.Book.Title.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(statusFilter))
            {
                if (statusFilter == "active")
                {
                    rentals = rentals.Where(r => !r.IsReturned);
                }
                else if (statusFilter == "returned")
                {
                    rentals = rentals.Where(r => r.IsReturned);
                }
            }

            // Apply sorting
            switch (sortOrder)
            {
                case "date_desc":
                    rentals = rentals.OrderByDescending(r => r.RentalDate);
                    break;
                case "return":
                    rentals = rentals.OrderBy(r => r.ReturnDate);
                    break;
                case "user":
                    rentals = rentals.OrderBy(r => r.User.Email);
                    break;
                default: // date ascending
                    rentals = rentals.OrderBy(r => r.RentalDate);
                    break;
            }

            return View(await PaginatedList<Rental>.CreateAsync(rentals, pageNumber, pageSize));
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


        public async Task<IActionResult> Users(
            string searchString,
            string sortOrder,
            bool includeInactive = false,
            int pageNumber = 1)
        {
            var pageSize = 10; // You can adjust this number
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentSort"] = sortOrder;
            ViewBag.IncludeInactive = includeInactive;

            var users = _userManager.Users.AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(searchString))
            {
                users = users.Where(u => 
                    u.Email.Contains(searchString) ||
                    u.FirstName.Contains(searchString) ||
                    u.LastName.Contains(searchString));
            }

            if (!includeInactive)
            {
                users = users.Where(u => u.IsActive);
            }

            // Apply sorting
            switch (sortOrder)
            {
                case "name_desc":
                    users = users.OrderByDescending(u => u.FirstName);
                    break;
                case "email":
                    users = users.OrderBy(u => u.Email);
                    break;
                case "email_desc":
                    users = users.OrderByDescending(u => u.Email);
                    break;
                default: // name ascending
                    users = users.OrderBy(u => u.FirstName);
                    break;
            }

            return View(await PaginatedList<AppUser>.CreateAsync(users, pageNumber, pageSize));
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

        public async Task<IActionResult> Books(
            string sortOrder,
            string currentFilter,
            string searchString,
            string categoryFilter,
            int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["TitleSortParm"] = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewData["AuthorSortParm"] = sortOrder == "author" ? "author_desc" : "author";
            ViewData["CategorySortParm"] = sortOrder == "category" ? "category_desc" : "category";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;
            ViewBag.CurrentCategory = categoryFilter;
            ViewBag.Categories = await _context.Categories.ToListAsync();

            var books = from b in _context.Books
                        select b;

            if (!String.IsNullOrEmpty(searchString))
            {
                books = books.Where(s => s.Title.Contains(searchString)
                                    || s.Author.Contains(searchString));
            }

            if (!String.IsNullOrEmpty(categoryFilter))
            {
                books = books.Where(s => s.Category.ToString() == categoryFilter);
            }

            books = sortOrder switch
            {
                "title_desc" => books.OrderByDescending(s => s.Title),
                "author" => books.OrderBy(s => s.Author),
                "author_desc" => books.OrderByDescending(s => s.Author),
                "category" => books.OrderBy(s => s.Category),
                "category_desc" => books.OrderByDescending(s => s.Category),
                _ => books.OrderBy(s => s.Title),
            };

            int pageSize = 10;
            return View(await PaginatedList<Book>.CreateAsync(books, pageNumber ?? 1, pageSize));
        }

        [HttpPost]
        public async Task<IActionResult> VerifyUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.IsVerified = true;
            await _userManager.UpdateAsync(user);

            // Mark related notification as read
            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.UserId == id && n.Type == "UserRegistration");
            if (notification != null)
            {
                notification.IsRead = true;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Users));
        }

        [HttpPost]
        public async Task<IActionResult> MarkNotificationAsRead(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification != null)
            {
                notification.IsRead = true;
                await _context.SaveChangesAsync();
            }
            return Ok();
        }

    }

}