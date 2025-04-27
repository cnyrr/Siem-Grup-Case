using API.DTOs;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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
        public required DateTime BirthDate { get; set; }

        // One-to-Many relationship with Book.
        [JsonIgnore]
        public ICollection<Book> Books { get; set; } = [];
    }
}
