using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public AuthorController(LibraryDbContext context)
        {
            _context = context;
        }

        // Fetch all authors from the database.
        // Return an empty list if no authors are found.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Author>>> GetAuthors()
        {
            var authors = await _context.Authors.ToListAsync();

            return Ok(authors);
        }

        // Fetch a specific author by ID.
        // Return a 404 error if the author is not found.
        [HttpGet("{id}")]
        public async Task<ActionResult<Author>> GetAuthor(int id)
        {
            var author = await _context.Authors.FindAsync(id);

            if (author == null)
            {
                return NotFound($"Author with ID {id} was not found.");
            }

            return Ok(author);
        }

        // Add a new author to the database.
        // Return a 400 error if the author object is null.
        // Return a 201 Created response with the location of the new author.
        [HttpPost]
        public async Task<ActionResult<Author>> PostAuthor(Author author)
        {
            if (author == null)
            {
                return BadRequest();
            }

            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, author);
        }

        // Update an existing author in the database.
        // Return a 400 error if the author ID in the URL does not match the ID in the body.
        // Return a 404 error if the author is not found.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthor(int id, Author author)
        {
            if (id != author.Id)
            {
                return BadRequest("Author ID in URL does not match the ID in the body.");
            }

            var existingAuthor = await _context.Authors.FindAsync(id);
            if (existingAuthor == null)
            {
                return NotFound();
            }

            existingAuthor.Name = author.Name;
            existingAuthor.BirthDate = author.BirthDate;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Delete an author from the database.
        // Return a 404 error if the author is not found.
        // Return a 204 No Content response if the deletion is successful.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
