// HomeController.cs
using Microsoft.AspNetCore.Mvc;
using CsvHelper;
using System.Globalization;
using WebApplication1.Models;
using WebApplication1.Data;
using WebApplication1.Abstracts;
using WebApplication1.Services;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<HomeController> _logger;
    private readonly IBookService _bookservice;

    public HomeController(ApplicationDbContext context, ILogger<HomeController> logger,IBookService bookservice)
    {
        _context = context;
        _logger = logger;
        _bookservice = bookservice;
    }

    public IActionResult Index()
    {
        var availableBooks =_bookservice.getAllBooks();
        return View(availableBooks);
       
    }

    // Add this new action for showing the upload form
    public IActionResult Import()
    {
        return View();
    }

 
}