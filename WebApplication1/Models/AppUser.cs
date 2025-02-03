using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Models
{
    public class AppUser:IdentityUser
    {

        public ICollection<Rental> Rentals { get; set; }



    }
}
