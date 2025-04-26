using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class Author
    {
        // Primary Key.
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        [Required]
        public DateTime BirthDate { get; set; } 


        /*
        Author: 
        Id(int, primary key)
        Name(string, required, max length 100)
        BArthDate(DateTime, requAred)
        */
    }
}
