using Microsoft.AspNetCore.Mvc;
using CsvHelper;
using System.Formats.Asn1;
using System.Globalization;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Services;
using WebApplication1.Abstracts;

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

        public async Task<IActionResult> Index()
        {
         var books=   await _bookservice.getAllBooks();
            return View(books);
        }


        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Book book)
        {
            if (ModelState.IsValid)
            {
                _bookservice.addBook(book);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                //todo throw new Exception("")
            }

            return View();
        }

        public async Task<IActionResult> Update(string id)
        {
            var book = await _bookservice.getBookById(id);
            return View(book);
        }


        [HttpPost]
        public async Task<IActionResult> Update(Book book)
        {

            if (ModelState.IsValid)
            {
              var updatedBook= await _bookservice.updateBook(book);
                return RedirectToAction("Index", "Home");
            }

            //todo hata ekle  

            return NotFound();
        }


    
        public async Task<IActionResult> Delete(string id)
        {
          
                 await  _bookservice.removeBook(id);
                return RedirectToAction("Index", "Home");
          
            
        }







    }
}