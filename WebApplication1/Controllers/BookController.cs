using Microsoft.AspNetCore.Mvc;
using CsvHelper;
using System.Formats.Asn1;
using System.Globalization;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Services;
using WebApplication1.Abstracts;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication1.Controllers
{
    public class BookController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBookService _bookservice;

        public BookController(ILogger<HomeController> logger,IBookService bookservice)
        {
            _logger = logger;
            _bookservice = bookservice;
         
        }
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Index()
        {
         var books=  await _bookservice.getAllBooks();
            return View(books);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Add()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Add(Book book)
        {
            if (ModelState.IsValid)
            {
                _bookservice.addBook(book);
                return RedirectToAction("Index", "Book");
            }
            else
            {
                //todo throw new Exception("")
            }

            return View();
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(string id)
        {
            var book = await _bookservice.getBookById(id);
            return View(book);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Update(Book book)
        {

            if (ModelState.IsValid)
            {
              var updatedBook= await _bookservice.updateBook(book);
                return RedirectToAction("Index", "Book");
            }

            //todo hata ekle  

            return NotFound();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
          
                 await  _bookservice.removeBook(id);
                return RedirectToAction("Index", "Book");
          
            
        }


        [Authorize(Roles = "User")]
        public async Task<IActionResult> Rent(string id)
        {

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            await _bookservice.rentBook(id, userId);
            return RedirectToAction("Index", "Book");


        }

    }
}