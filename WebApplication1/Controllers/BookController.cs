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

namespace WebApplication1.Controllers
{
    public class BookController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBookService _bookService;

        public BookController(ILogger<HomeController> logger, IBookService bookService)
        {
            _logger = logger;
            _bookService = bookService;
        }

       
        public async Task<IActionResult> Index(string? titleFilter, string? categoryFilter)
        {
            ViewBag.TitleFilter = titleFilter;
            ViewBag.CategoryFilter = categoryFilter;
            var categories = await _bookService.GetDistinctCategories();
            ViewBag.Categories = categories;

            var books = await _bookService.GetFilteredBooks(titleFilter, categoryFilter);
            return View(books);
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
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

          
            
                await _bookService.rentBook(id, userId);
                return RedirectToAction(nameof(Index));
            
          
        }
    }
}