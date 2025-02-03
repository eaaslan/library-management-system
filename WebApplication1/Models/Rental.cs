namespace WebApplication1.Models
{
    public class Rental
    {
        public string Id { get; set; }
        public Book Book { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public string BookISBN { get; set; }
        public DateTime RentalDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public bool IsReturned { get; set; }

    }
}
