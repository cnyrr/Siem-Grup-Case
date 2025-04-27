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
        public int PublishedYear { get; set; }

        // Foreign Key from Author.
        public required int AuthorId { get; set; }

        // Navigation property.
        [JsonIgnore]
        public required Author Author { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}
