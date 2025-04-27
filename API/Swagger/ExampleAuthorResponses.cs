using API.Models;
using Swashbuckle.AspNetCore.Filters;

namespace API.Swagger
{
    public class ExampleEmptyListAuthorResponse : IExamplesProvider<IEnumerable<Author>>
    {
        public IEnumerable<Author> GetExamples()
        {
            return new List<Author>();
        }
    }

    public class ExampleAuthorResponse : IExamplesProvider<Author>
    {
        public Author GetExamples()
        {
            return new Author
            {
                Id = 1,
                Name = "J. R. R. Tolkien",
                BirthDate = new DateTime(1892, 1, 3)
            };
        }
    }

    public class ExampleAuthorListResponse : IExamplesProvider<IEnumerable<Author>>
    {
        public IEnumerable<Author> GetExamples()
        {
            return new List<Author>
            {
                new Author
                {
                    Id = 1,
                    Name = "J. R. R. Tolkien",
                    BirthDate = new DateTime(1892, 1, 3)
                },
                new Author
                {
                    Id = 2,
                    Name = "H. P. Lovecraft",
                    BirthDate = new DateTime(1890, 8, 20)
                }
            };
        }
    }

    public class ExampleAuthorNotFoundResponse : IExamplesProvider<string>
    {
        public string GetExamples()
        {
            return "Author with ID 1 was not found.";
        }
    }

    public class ExampleAuthorNullResponse : IExamplesProvider<string>
    {
        public string GetExamples()
        {
            return "Author object is null.";
        }
    }

    public class ExampleAuthorIDExistsResponse : IExamplesProvider<string>
    {
        public string GetExamples()
        {
            return "Author with the same ID already exists.";
        }
    }

    public class ExampleAuthorHasBooksResponse : IExamplesProvider<string>
    {
        public string GetExamples()
        {
            return "Author has books and cannot be deleted.";
        }
    }

    public class ExampleAuthorIDMismatchResponse : IExamplesProvider<string>
    {
        public string GetExamples()
        {
            return "Author ID in the URL does not match the ID in the request body.";
        }
    }
}
