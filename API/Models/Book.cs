using static System.Runtime.InteropServices.JavaScript.JSType;
using System;
using System.ComponentModel.DataAnnotations;

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

        // Foreign key from Author.
        [Required]
        public int AuthorId { get; set; }

        [Required]
        public decimal Price { get; set; }
        /*
        Book: 
        Id(int, primary key)
        Title(string, required, max length 200)
        PublishedYear(int, required)
        AuthorId(foreign key from Author)
        Price(decimal, required)
        */
    }
}
