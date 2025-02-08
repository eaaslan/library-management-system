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

        public DateTime DueDate { get; set; }

        public DateTime? ReturnDate { get; set; }  // it can be nullable for some cases

        public bool IsReturned { get; set; } = false; 

        public bool IsOverdue => !IsReturned && 
           DateTime.UtcNow.Date > DueDate.Date;

        public decimal? LateFee { get; set; }  //TODO Add this for potential future use

        public RentalStatus Status 
        {
            get 
            {
                if (IsReturned) 
                    return RentalStatus.Returned;
                return IsOverdue ? RentalStatus.Overdue : RentalStatus.Active;
            }
        }

        // Ensure dates are always stored in UTC
        public void SetRentalDate(DateTime date)
        {
            RentalDate = DateTime.SpecifyKind(date.Date, DateTimeKind.Utc);
            DueDate = DateTime.SpecifyKind(date.Date.AddDays(14), DateTimeKind.Utc);
        }

        public void SetReturnDate(DateTime date)
        {
            ReturnDate = DateTime.SpecifyKind(date.Date, DateTimeKind.Utc);
        }
    }

    public enum RentalStatus
    {
        Active,
        Returned,
        Overdue
    }
}
