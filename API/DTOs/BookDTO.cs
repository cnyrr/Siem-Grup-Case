using API.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.DTOs
{
    public class BookDTO
    {
        public int? Id { get; set; }

        [Required]
        [MaxLength(200)]
        public required string Title { get; set; }

        [Required]
        [Range(1, 9999)]
        public required int PublishedYear { get; set; }

        // Foreign Key from Author.
        public required int AuthorId { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
    }
}
