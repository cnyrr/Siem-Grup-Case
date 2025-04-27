using static System.Runtime.InteropServices.JavaScript.JSType;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.Models
{
    public class Book
    {
        // Primary Key.
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public required string Title { get; set; }

        [Required]
        public int PublishedYear { get; set; }

        // Foreign Key to Author.
        public required int AuthorId { get; set; }

        // Avoids circular reference in API.
        [JsonIgnore]
        public required Author Author { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}
