// Refactored code after 1 - Simple Checks
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using ImporterConsole1Spike;

// Make fake Author and Book data from firstname, surname, and word
// text files for testing BookTech website
namespace TwoRenameRefactored
{
    class Program
    {
        static void MainTwoRefactored()
        {
            var firstnames = File.ReadAllLines(@"firstnames.txt").ToList();
            var surnames = File.ReadAllLines(@"surnames.txt").ToList();
            var words = File.ReadAllLines(@"words.txt").ToList();
            Console.WriteLine(firstnames.Count + " firstnames loaded");
            Console.WriteLine(surnames.Count + " surnames loaded");
            Console.WriteLine(words.Count + " book title words loaded");
            var rnd = new Random();
            var cs = ConfigurationManager.ConnectionStrings["BookTechConnectionString"].ConnectionString;
            using (var cnn = new SqlConnection(cs))
            {
                cnn.Open();
                // Insert Authors
                for (int i = 0; i < 5; i++)
                {
                    int authorID;
                    using (var cmd = new SqlCommand("INSERT INTO [Authors](FirstName, LastName,EmailAddress) VALUES(@FirstName, @LastName, @EmailAddress); SELECT SCOPE_IDENTITY()", cnn))
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
                        using (var cmd = new SqlCommand("INSERT INTO Books(Title, AuthorID) VALUES(@Title, @AuthorID)", cnn))
                        {
                            string firstWord = words[rnd.Next(words.Count)].CapitaliseFirstLetter();
                            string secondWord = words[rnd.Next(words.Count)].CapitaliseFirstLetter();
                            string bookTitle = firstWord + " of the " + secondWord;
                            cmd.Parameters.AddWithValue("@Title", bookTitle);
                            cmd.Parameters.AddWithValue("@AuthorID", authorID);
                            cmd.ExecuteScalar();
                        }
                    }
                }
            }
        }
    }
}