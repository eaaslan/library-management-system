using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Book
    {
        [Key]
        public string Id { get; set; }  // ISBN as ID


        [Required]
        public string Title { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string? ImageUrl { get; set; }

        public bool Available { get; set; }

        public bool isDeleted { get; set; }

        public ICollection<Rental>? Rentals { get; set; }
    }
}
