using WebApplication1.Models;
using System.ComponentModel.DataAnnotations.Schema;

public class Notification
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsRead { get; set; }
    public string Type { get; set; } // e.g., "UserRegistration", "BookRental", etc.
    public string UserId { get; set; }

    [ForeignKey("UserId")]
    public virtual AppUser User { get; set; }
} 