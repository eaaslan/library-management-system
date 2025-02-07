using WebApplication1.Models;

namespace WebApplication1.Models.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int TotalBooks { get; set; }
        public int ActiveBooks { get; set; }
        public int DeletedBooks { get; set; }
        public int TotalUsers { get; set; }
        public int DeletedUsers { get; set; }
        public int TotalRentals { get; set; }
        public int ActiveRentals { get; set; }
        public int OverdueRentals { get; set; }
        public int TotalCategories { get; set; }
        public List<Rental> RecentRentals { get; set; } = new List<Rental>();
    }
} 