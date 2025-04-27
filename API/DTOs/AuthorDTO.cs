using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class AuthorDTO
    {
        public required string Name { get; set; }

        [MaxLength(100)]
        public required DateTime BirthDate { get; set; }
    }
}
