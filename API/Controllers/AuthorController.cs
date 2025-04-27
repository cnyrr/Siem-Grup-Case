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
        public async Task<ActionResult<Author>> PostAuthor([FromBody] AuthorDTO author_dto)
        {
            // Validate the author object.
            if (author_dto == null)
            {
                return BadRequest("Author object is null.");
            }

            // Check if the id already exists.
            if (author_dto.Id.HasValue)
            {
                
                var existingAuthor = await _context.Authors.FindAsync(author_dto.Id);

                if (existingAuthor != null)
                {
                    return Conflict($"Author with ID {author_dto.Id} already exists.");
                }
            }

            var author = new Author
            {
                Id = author_dto.Id ?? 0,
                Name = author_dto.Name,
                BirthDate = author_dto.BirthDate
            };

            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, author);
        }

        /// <summary>
        /// Update an existing author in the database.
        /// </summary>
        /// <response code="200">Responds with the updated author.</response>
        /// <response code="400">Author object is null.</response>
        /// <response code="404">Author with the given ID was not found.</response>
        /// <response code="409">Author object is null or the ID in the URL does not match the ID in the body.</response>
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Author), StatusCodes.Status200OK, "application/json")]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest, "text/plain")]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound, "text/plain")]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict, "text/plain")]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ExampleAuthorNullResponse))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(ExampleAuthorNotFoundResponse))]
        [SwaggerResponseExample(StatusCodes.Status409Conflict, typeof(ExampleAuthorIDMismatchResponse))]
        public async Task<IActionResult> PutAuthor(int id, Author author)
        {
            // Check if the author exists.
            var existingAuthor = await _context.Authors.FindAsync(id);
            if (existingAuthor == null)
            {
                return NotFound();
            }

            // Check if author is null.
            if (author == null)
            {
                return BadRequest("Author object is null.");
            }

            // Check if the author ID in the URL matches the ID in the body.
            if (author.Id != id && author.Id != 0)
            {
                return Conflict("Author ID in URL does not match the ID in the body.");
            }

            existingAuthor.Name = author.Name;
            existingAuthor.BirthDate = author.BirthDate;

            await _context.SaveChangesAsync();

            return Ok(existingAuthor);
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
