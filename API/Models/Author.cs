using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class Author
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        // One-to-Many relationship with Book.
        public ICollection<Book> Books { get; set; } = [];
    }
}
