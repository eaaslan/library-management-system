using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class AdminBaseController : Controller
    {
        protected readonly ApplicationDbContext _context;

        public AdminBaseController(ApplicationDbContext context)
        {
            _context = context;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            
            var unreadCount = _context.Notifications.Count(n => !n.IsRead);
            var recentNotifications = _context.Notifications
                .Include(n => n.User)
                .OrderByDescending(n => n.CreatedAt)
                .Take(5)
                .ToList();

            ViewBag.UnreadNotifications = unreadCount;
            ViewBag.Notifications = recentNotifications;
        }
    }
} 