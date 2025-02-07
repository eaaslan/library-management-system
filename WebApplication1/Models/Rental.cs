using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Rental
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int BookId { get; set; }  

        [ForeignKey("BookId")]
        public virtual Book Book { get; set; }
        // use virtual to lazy load on navigation properties
        //check is it necessary to use foreign key notation

        [Required]
        public string ISBN { get; set; } 

        [Required]
        public string UserId { get; set; } 

        [ForeignKey("UserId")]
        public virtual AppUser User { get; set; }

        [Required]
        public DateTime RentalDate { get; set; }

        public DateTime? ReturnDate { get; set; }  // it can be nullable for some cases

        public bool IsReturned { get; set; } = false; 
    }
}
