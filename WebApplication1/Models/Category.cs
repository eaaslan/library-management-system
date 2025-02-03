using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace WebApplication1.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        public virtual ICollection<Book>? Books { get; set; }

        public override string ToString() // just return the name of the category
        {
            return Name;
        }
    }
}
