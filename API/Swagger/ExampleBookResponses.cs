using API.DTOs;
using API.Models;
using Swashbuckle.AspNetCore.Filters;

namespace API.Swagger
{
    public class ExampleBookResponses
    {
        public class ExampleBookDTOResponse : IExamplesProvider<BookDTO>
        {
            public BookDTO GetExamples()
            {
                return new BookDTO
                {
                    Id = 0,
                    Title = "The Hobbit",
                    PublishedYear = 1937,
                    AuthorId = 1,
                    Price = 12.99m
                };
            }
        }
        public class ExampleBookResponse : IExamplesProvider<Book>
        {
            public Book GetExamples()
            {
                return new Book
                {
                    Id = 2,
                    Title = "The Hobbit",
                    PublishedYear = 1937,
                    AuthorId = 1,
                    Price = 12.99m
                };
            }
        }
        public class ExampleBookListResponse : IExamplesProvider<List<Book>>
        {
            public List<Book> GetExamples()
            {
                return new List<Book>
                {
                    new Book
                    {
                        Id = 1,
                        Title = "The Great Gatsby",
                        PublishedYear = 1925,
                        AuthorId = 1,
                        Price = 10.99m
                    },
                    new Book
                    {
                        Id = 2,
                        Title = "To Kill a Mockingbird",
                        PublishedYear = 1960,
                        AuthorId = 2,
                        Price = 12.99m
                    }
                };
            }
        }

        public class ExampleBookNotFoundResponse : IExamplesProvider<string>
        {
            public string GetExamples()
            {
                return "Book with ID 1 was not found.";
            }
        }

        public class ExampleBookNullResponse : IExamplesProvider<string>
        {
            public string GetExamples()
            {
                return "Book object is null.";
            }
        }

        public class ExampleBookAuthorMissingResponse : IExamplesProvider<string>
        {
            public string GetExamples()
            {
                return "Author with ID 1 does not exist. Cannot create book without an author.";
            }
        }

        public class ExampleBookIDExistsResponse : IExamplesProvider<string>
        {
            public string GetExamples()
            {
                return "Book with ID 1 already exists.";
            }
        }

        public class ExampleBookOrAuthorMissingResponse : IExamplesProvider<string>
        {
            public string GetExamples()
            {
                return $"Author with ID 1 does not exist. Cannot create book without an author.";
            }
        }

        public class ExampleBookIDMismatchResponse : IExamplesProvider<string>
        {
            public string GetExamples()
            {
                return "Book ID in the URL does not match the ID in the body.";
            }
        }
    }
}
