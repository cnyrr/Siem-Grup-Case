using static System.Runtime.InteropServices.JavaScript.JSType;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public required string Title { get; set; }

        [Required]
        [Range(1, 9999)]
        public required int PublishedYear { get; set; }

        // Foreign Key from Author.
        public required int AuthorId { get; set; }

        // Navigation property.
        [JsonIgnore]
        public Author? Author { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public required decimal Price { get; set; }
    }
}
