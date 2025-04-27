using API.DTOs;
using API.Models;
using API.Swagger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Filters;

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

        /// <summary>
        /// Fetch all authors from the database.
        /// </summary>
        /// <response code="200">Returns the list of authors.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<Author>), StatusCodes.Status200OK, "application/json")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ExampleAuthorListResponse))]
        public async Task<ActionResult<IEnumerable<Author>>> GetAuthors()
        {
            var authors = await _context.Authors.ToListAsync();

            return Ok(authors);
        }

        /// <summary>
        /// Fetch the author with given id.
        /// </summary>
        /// <response code="200">Returns the author with the given ID.</response>
        /// <response code="404">Author with the given ID was not found.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Author), StatusCodes.Status200OK, "application/json")]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound, "text/plain")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ExampleAuthorResponse))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(ExampleAuthorNotFoundResponse))]
        public async Task<ActionResult<Author>> GetAuthor(int id)
        {
            var author = await _context.Authors.FindAsync(id);

            if (author == null)
            {
                return NotFound($"Author with ID {id} was not found.");
            }

            return Ok(author);
        }

        /// <summary>
        /// Add a new author to the database.
        /// </summary>
        /// <response code="201">Responds with the location of the new author on success.</response>
        /// <response code="400">Author object is null.</response>
        /// <response code="409">Author with the same ID already exists.</response>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Author), StatusCodes.Status201Created, "application/json")]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest, "text/plain")]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict, "text/plain")]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ExampleAuthorNullResponse))]
        [SwaggerResponseExample(StatusCodes.Status409Conflict, typeof(ExampleAuthorIDExistsResponse))]
        public async Task<ActionResult<Author>> PostAuthor([FromBody] CreateAuthorDto authorDTO)
        {
            // Validate the author object.
            if (authorDTO == null)
            {
                return BadRequest("Author object is null.");
            }

            // Check if the id already exists.
            if (authorDTO.Id.HasValue)
            {
                
                var existingAuthor = await _context.Authors.FindAsync(authorDTO.Id);

                if (existingAuthor != null)
                {
                    return Conflict($"Author with ID {authorDTO.Id} already exists.");
                }
            }

            var author = new Author
            {
                Id = authorDTO.Id ?? 0,
                Name = authorDTO.Name,
                BirthDate = authorDTO.BirthDate
            };

            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, author);
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
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

        /// <summary>
        /// Delete an author from the database.
        /// </summary>
        /// <response code="204">Author deleted successfully.</response>
        /// <response code="404">Author with the given ID was not found.</response>
        /// <response code="409">Author has books and cannot be deleted.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound, "text/plain")]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict, "text/plain")]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(ExampleAuthorNotFoundResponse))]
        [SwaggerResponseExample(StatusCodes.Status409Conflict, typeof(ExampleAuthorHasBooksResponse))]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            // Check if the author exists.
            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound($"Author with ID {id} was not found.");
            }

            // Check if the author has any books associated with them.
            if (author.Books.Count != 0)
            {
                return Conflict("Author has books and cannot be deleted.");
            }

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
