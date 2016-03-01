using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ImporterConsole1Spike
{
    class Program
    {
        static void MainOne(string[] args)
        {
            // goal is to get many 10,000's or millions of real book and author data
            // so can demo refactoring
            // and is useful in BookTech demo (needing paging filtering sorting etc..)
            // and has a real application amount of data.
            // useful for Proof of Concept testing
            // firstname (Given-Names)and surnames (ASSurnames) from http://www.outpost9.com/files/WordLists.html
            // words came from: http://www.manythings.org/vocabulary/lists/l/words.php?f=noll04
            //     random word.. of the ...   random word
            var firstnames = File.ReadAllLines(@"firstnames.txt").ToList();
            //foreach (var line in firstNames)
            //{
            //    Console.WriteLine(line);
            //}
            var surnames = File.ReadAllLines(@"surnames.txt").ToList();
            //foreach (var surname in surnames) Console.WriteLine(surname);
            var words = File.ReadAllLines(@"words.txt").ToList();
            //foreach (var word in words.Take(50))
            //{
            //    Console.WriteLine(word);
            //}
            var rnd = new Random();
            for (int i = 0; i < 10; i++)
            {
                // make an Author Name
                string authorFirstname = firstnames[rnd.Next(firstnames.Count)];
                string authorSurname = surnames[rnd.Next(surnames.Count)].CapitaliseFirstLetter();
                Console.WriteLine(authorFirstname + " " + authorSurname);
            }
            Console.WriteLine();
            // Make a book title
            //  eg word of the word  - Lord of the Rings
            for (int i = 0; i < 10; i++)
            {
                string firstWord = words[rnd.Next(words.Count)].CapitaliseFirstLetter();
                string secondWord = words[rnd.Next(words.Count)].CapitaliseFirstLetter();
                string bookTitle = firstWord + " of the " + secondWord;
                Console.WriteLine(bookTitle);
            }
            // Insert Authors and Books into the database
            //  data source=.\;initial catalog=BookTech;integrated security=True;MultipleActiveResultSets=True;
            var connectionString = ConfigurationManager.ConnectionStrings["BookTechConnectionString"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // insert 100 authors
                for (int k = 0; k < 10; k++)
                {
                    int authorID = 0;
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
                    // Add a random number of books between 1 and 5 for this author
                    for (int i = 0; i < rnd.Next(1, 7); i++)
                    {
                        using (var cmd = new SqlCommand("INSERT INTO Books(Title, AuthorID) VALUES(@Title, @AuthorID); SELECT SCOPE_IDENTITY()", connection))
                        {
                            string firstWord = words[rnd.Next(words.Count)].CapitaliseFirstLetter();
                            string secondWord = words[rnd.Next(words.Count)].CapitaliseFirstLetter();
                            string bookTitle = firstWord + " of the " + secondWord;
                            cmd.Parameters.AddWithValue("@Title", bookTitle);
                            cmd.Parameters.AddWithValue("@AuthorID", authorID);
                            var bookID = Convert.ToInt32(cmd.ExecuteScalar());
                        }
                    }
                }

            }
        }
    }
    public static class Util
    {
        public static string CapitaliseFirstLetter(this string s)
        {
            if (String.IsNullOrEmpty(s)) return s;
            if (s.Length == 1) return s.ToUpper();
            return s.Remove(1).ToUpper() + s.Substring(1);
        }
    }
}