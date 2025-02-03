using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Models
{
    public class AppRole:IdentityRole
    {

        public const string Admin = "Admin";
        public const string User = "User";
        //todo at staff later
    }
}
