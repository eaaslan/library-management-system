using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(13)]
        [Display(Name = "ISBN")]
        public string ISBN { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(100)]
        [Display(Name = "Author")]
        public string? Author { get; set; } = "Unknown Author";

        [StringLength(2000)]
        [Display(Name = "Description")]
        public string? Description { get; set; } = "No description available";

        [StringLength(300)]
        [Display(Name = "Image URL")]
        public string? ImageUrl { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public bool Available { get; set; } = true;

        public bool IsDeleted { get; set; } = false;

        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }//FIX: why cant update without making this nullable ?

        public virtual ICollection<Rental>? Rentals { get; set; }
    }
}
