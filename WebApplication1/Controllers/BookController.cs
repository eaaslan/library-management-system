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

        [Authorize(Roles = "Admin,User")]
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

            var categories = await _bookService.GetDistinctCategories();
            _logger.LogInformation("Loading book for update - Book ID: {BookId}, CategoryId: {CategoryId}, Category Name: {CategoryName}", 
                book.Id, book.CategoryId, book.Category?.Name);
            _logger.LogInformation("Available categories: {Categories}", 
                string.Join(", ", categories.Select(c => $"{c.Id}:{c.Name}")));

            ViewBag.Categories = categories;
            return View(book);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Update(Book book)
        {
            try
            {
                _logger.LogInformation("Attempting to update book - Book ID: {BookId}, CategoryId: {CategoryId}, Title: {Title}", 
                    book.Id, book.CategoryId, book.Title);

                // Log all form values
                foreach (var key in Request.Form.Keys)
                {
                    _logger.LogInformation("Form value - {Key}: {Value}", key, Request.Form[key].ToString());
                }
                
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state when updating book {BookId}", book.Id);
                    foreach (var modelStateEntry in ModelState.Values)
                    {
                        foreach (var error in modelStateEntry.Errors)
                        {
                            _logger.LogWarning("Validation error: {ErrorMessage}", error.ErrorMessage);
                        }
                    }

                    // Log model state for CategoryId specifically
                    if (ModelState.ContainsKey("CategoryId"))
                    {
                        var categoryState = ModelState["CategoryId"];
                        _logger.LogWarning("CategoryId state - Raw value: {RawValue}, Attempted value: {AttemptedValue}", 
                            categoryState.RawValue, categoryState.AttemptedValue);
                    }

                    var categories = await _bookService.GetDistinctCategories();
                    _logger.LogInformation("Categories count: {Count}, Selected CategoryId: {CategoryId}", 
                        categories.Count, book.CategoryId);
                    ViewBag.Categories = categories;
                    return View(book);
                }

                var updatedBook = await _bookService.updateBook(book);
                _logger.LogInformation("Successfully updated book {BookId} with CategoryId {CategoryId}", 
                    updatedBook.Id, updatedBook.CategoryId);
                TempData["Success"] = "Book updated successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating book {BookId}", book.Id);
                ModelState.AddModelError("", $"Error updating book: {ex.Message}");
                ViewBag.Categories = await _bookService.GetDistinctCategories();
                return View(book);
            }
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

            try
            {
                await _bookService.rentBook(id, userId);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error renting book");
                TempData["Error"] = "Could not rent the book. Please try again later.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}