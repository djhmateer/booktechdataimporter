using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;


namespace ImporterConsole
{
    // C# Language definitions helper
    public class Book
    {
        // Property
        public string Title { get; set; }

        // Field / private property / private variable
        private string _title;

        // Constuctor
        public Book(){}

    }

    // BookRepository implements IBookRepository
    public class BookRepository : IBookRepository
    {
        private readonly ILogger _log;

        // Pure (Poor mans) constructor dependency injection
        public BookRepository(ILogger log)
        {
            _log = log;
        }

        // Method with argument
        public void SaveBook(Book book)
        {
            _log.Debug("Saving book");
        }
    }

    public interface ILogger{void Debug(string message);}

    public interface IBookRepository{}

    // BookController is a BaseController (inheritance)
    public class BookController : BaseController
    {
        // Field
        // BookController has a BookRepository (composition)
        private readonly IBookRepository _repo;

        
        public BookController(IBookRepository repo)
        {
            _repo = repo;
        }

        public ViewResult BookEdit(Book book)
        {
            // Passing an argument/parameter
            _repo.SaveBook(book);
        }
    }

    public class BaseController { }


    // Get large quantities of Author and Book data for testing BookTech website
    // Needed for load testing, paging, filtering, sorting 
    class Program
    {
        static void Main()
        {
            var firstnames = File.ReadAllLines(@"firstnames.txt").ToList();
            var surnames = File.ReadAllLines(@"surnames.txt").ToList();
            var words = File.ReadAllLines(@"words.txt").ToList();

            DisplaySummaryCountsOfLoadedFiles(firstnames, surnames, words);

            var rnd = new Random();

            using (var connection = new SqlConnection(GetConnectionString()))
            {
                connection.Open();
                // Insert Authors
                for (int i = 0; i < 5; i++)
                {
                    int authorID;
                    using (var cmd = new SqlCommand("INSERT INTO [Authors](FirstName, LastName,EmailAddress) VALUES(@FirstName, @LastName, @EmailAddress); SELECT SCOPE_IDENTITY()", connection))
                    {
                        string authorFirstname = firstnames[rnd.Next(firstnames.Count)];
                        string authorSurname = surnames[rnd.Next(surnames.Count)].CapitaliseFirstLetter();
                        string autorEmail = authorFirstname + "@" + authorSurname + ".com";
                        cmd.Parameters.AddWithValue("@FirstName", authorFirstname);
                        cmd.Parameters.AddWithValue("@LastName", authorSurname);
                        cmd.Parameters.AddWithValue("@EmailAddress", autorEmail);
                        authorID = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    // Add random number of books to the newly created Author
                    for (int j = 0; j < rnd.Next(1, 7); j++)
                    {
                        using (var cmd = new SqlCommand("INSERT INTO Books(Title, AuthorID) VALUES(@Title, @AuthorID)", connection))
                        {
                            var bookTitle = GenerateBookTitle(words, rnd);

                            cmd.Parameters.AddWithValue("@Title", bookTitle);
                            cmd.Parameters.AddWithValue("@AuthorID", authorID);

                            cmd.ExecuteScalar();
                        }
                    }
                }
            }
        }

        private static string GetConnectionString()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["BookTechConnectionString"].ConnectionString;
            return connectionString;
        }

        private static void DisplaySummaryCountsOfLoadedFiles(List<string> firstnames, List<string> surnames, List<string> words)
        {
            Console.WriteLine(firstnames.Count + " firstnames loaded");
            Console.WriteLine(surnames.Count + " surnames loaded");
            Console.WriteLine(words.Count + " book title words loaded");
        }

        private static string GenerateBookTitle(List<string> words, Random rnd)
        {
            string firstWord = words[rnd.Next(words.Count)].CapitaliseFirstLetter();
            string secondWord = words[rnd.Next(words.Count)].CapitaliseFirstLetter();
            string bookTitle = firstWord + " of the " + secondWord;
            return bookTitle;
        }
    }
}
