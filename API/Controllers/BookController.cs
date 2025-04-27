using API.DTOs;
using API.Models;
using API.Swagger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Filters;
using static API.Swagger.ExampleBookResponses;

namespace API.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public BookController(LibraryDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Fetch all books from the database.
        /// </summary>
        /// <response code="200">Returns the list of books.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<Book>), StatusCodes.Status200OK, "application/json")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ExampleBookListResponse))]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            var books = await _context.Books.ToListAsync();

            return Ok(books);
        }

        /// <summary>
        /// Fetch the book with given id.
        /// </summary>
        /// <response code="200">Returns the book with the given ID.</response>
        /// <response code="404">Book with the given ID was not found.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Book), StatusCodes.Status200OK, "application/json")]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound, "text/plain")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ExampleBookResponses))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(ExampleBookNotFoundResponse))]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound($"Book with ID {id} was not found.");
            }

            return Ok(book);
        }

        /// <summary>
        /// Add a new book to the database.
        /// </summary>
        /// <response code="201">Responds with the location of the new book on success.</response>
        /// <response code="400">Book object is null.</response>
        /// <response code="404">Book or Author ID doesn't exists. Cannot create book without an author.</response>
        /// <response code="409">Book with the same ID already exists.</response>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Author), StatusCodes.Status201Created, "application/json")]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest, "text/plain")]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound, "text/plain")]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict, "text/plain")]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ExampleBookNullResponse))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(ExampleBookOrAuthorMissingResponse))]
        [SwaggerResponseExample(StatusCodes.Status409Conflict, typeof(ExampleBookIDExistsResponse))]
        public async Task<ActionResult<Book>> PostBook([FromBody] BookDTO book_dto)
        {
            // Validate the book object.
            if (book_dto == null)
            {
                return BadRequest("Book object is null.");
            }

            // Check if the id already exists.
            if (book_dto.Id.HasValue)
            {

                var existingBook = await _context.Books.FindAsync(book_dto.Id);

                if (existingBook != null)
                {
                    return Conflict($"Book with ID {book_dto.Id} already exists.");
                }
            }

            // Check if the author exists.
            var author = await _context.Authors.FindAsync(book_dto.AuthorId);
            if (author == null)
            {
                return NotFound($"Author with ID {book_dto.AuthorId} does not exist. Cannot create book without an author.");
            }

            var book = new Book
            {
                Title = book_dto.Title,
                PublishedYear = book_dto.PublishedYear,
                AuthorId = book_dto.AuthorId,
                Author = author,
                Price = book_dto.Price
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }

        /// <summary>
        /// Update an existing book in the database.
        /// </summary>
        /// <response code="200">Returns the updated book.</response>
        /// <response code="400">Book object is null.</response>
        /// <response code="404">Book with the given ID was not found.</response>
        /// <response code="409">Book ID mismatch between URL and body.</response>
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Book), StatusCodes.Status200OK, "application/json")]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest, "text/plain")]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound, "text/plain")]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict, "text/plain")]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ExampleBookNullResponse))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(ExampleBookNotFoundResponse))]
        [SwaggerResponseExample(StatusCodes.Status409Conflict, typeof(ExampleBookIDMismatchResponse))]
        public async Task<IActionResult> PutBook(int id, [FromBody] BookDTO book_dto)
        {
            // Check if the book object is null.
            if (book_dto == null)
            {
                return BadRequest("Book object is null.");
            }

            // Check if the book exists.
            var existingBook = await _context.Books.FindAsync(id);
            if (existingBook == null)
            {
                return NotFound($"Book with ID {id} was not found.");
            }

            // Check if the ID in the URL matches the ID in the body.
            if (book_dto.Id.HasValue && book_dto.Id != id)
            {
                return Conflict("Book ID in URL does not match the ID in the body.");
            }

            // Check whether or not the author ID has changed and if so, check if the new author exists.
            if (book_dto.AuthorId != existingBook.AuthorId)
            {
                var author = await _context.Authors.FindAsync(book_dto.AuthorId);
                if (author == null)
                {
                    return Conflict($"Author with ID {book_dto.AuthorId} does not exist. Cannot update book without a valid author.");
                }
            }

            existingBook.Title = book_dto.Title;
            existingBook.PublishedYear = book_dto.PublishedYear;
            existingBook.AuthorId = book_dto.AuthorId;
            existingBook.Price = book_dto.Price;

            await _context.SaveChangesAsync();

            return Ok(existingBook);
        }

        /// <summary>
        /// Delete a book from the database.
        /// </summary>
        /// <response code="204">Book deleted successfully.</response>
        /// <response code="404">Book with the given ID was not found.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound, "text/plain")]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(ExampleBookNotFoundResponse))]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound($"Book with ID {id} was not found.");
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
