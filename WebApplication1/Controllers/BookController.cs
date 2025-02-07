using Microsoft.AspNetCore.Mvc;
using CsvHelper;
using System.Formats.Asn1;
using System.Globalization;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Services;
using WebApplication1.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace WebApplication1.Controllers
{
    public class BookController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBookService _bookService;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public BookController(ILogger<HomeController> logger, IBookService bookService, ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _logger = logger;
            _bookService = bookService;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(
            string sortOrder,
            string currentFilter,
            string searchString,
            string categoryFilter,
            string availabilityFilter,
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
            ViewBag.CurrentAvailability = availabilityFilter;
            ViewBag.Categories = await _context.Categories.ToListAsync();

            var books = _context.Books
                .Include(b => b.Category)  // Include the Category navigation property
                .Where(b => !b.IsDeleted);

            // Apply filters
            if (!String.IsNullOrEmpty(searchString))
            {
                books = books.Where(s => s.Title.Contains(searchString)
                                    || s.Author.Contains(searchString));
            }

            if (!String.IsNullOrEmpty(categoryFilter))
            {
                books = books.Where(b => b.Category.Name == categoryFilter);  // Use Category.Name instead of ToString()
            }

            // Add availability filter
            if (!String.IsNullOrEmpty(availabilityFilter))
            {
                if (availabilityFilter == "available")
                {
                    books = books.Where(b => b.Available);
                }
                else if (availabilityFilter == "rented")
                {
                    books = books.Where(b => !b.Available);
                }
            }

            // Apply sorting
            books = sortOrder switch
            {
                "title_desc" => books.OrderByDescending(s => s.Title),
                "author" => books.OrderBy(s => s.Author),
                "author_desc" => books.OrderByDescending(s => s.Author),
                "category" => books.OrderBy(s => s.Category.Name),  // Use Category.Name
                "category_desc" => books.OrderByDescending(s => s.Category.Name),  // Use Category.Name
                _ => books.OrderBy(s => s.Title),
            };

            int pageSize = 8;
            return View(await PaginatedList<Book>.CreateAsync(books, pageNumber ?? 1, pageSize));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add()
        {
            ViewBag.Categories = await _bookService.GetDistinctCategories();
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Add(Book book)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _bookService.GetDistinctCategories();
                return View(book);
            }

            try
            {
                await _bookService.addBook(book);
                TempData["Success"] = "Book added successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error adding book: " + ex.Message);
                ViewBag.Categories = await _bookService.GetDistinctCategories();
                return View(book);
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id)
        {
            var book = await _bookService.getBookById(id);
            if (book == null)
            {
                return NotFound();
            }

            ViewBag.Categories = await _bookService.GetDistinctCategories();
            return View(book);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Update(Book book)
        {
          
                if (!ModelState.IsValid)
                {  
                    return View(book);
                }

              var updatedBook = await _bookService.updateBook(book);
                return RedirectToAction(nameof(Index));
         
       
          
               
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _bookService.removeBook(id);
            if (!result)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> Rent(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (!user.IsVerified)
            {
                TempData["Error"] = "Your account needs to be verified before you can rent books.";
                return RedirectToAction(nameof(Index));
            }

            await _bookService.rentBook(id, userId);
            return RedirectToAction(nameof(Index));
        }
    }
}