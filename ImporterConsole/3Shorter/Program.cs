using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;


namespace Three
{
    // Make fake Author and Book data from firstname, surname, and word
    // text files for testing BookTech website
    class Program
    {
        static void Main()
        {
            for (int i = 0; i < 5; i++)
            {
                var authorID = InsertRandomlyNamedAuthorAndReturnID();
                AssignRandomNumberOfBookTitlesToAuthor(authorID);
            }
        }

        private static void AssignRandomNumberOfBookTitlesToAuthor(int authorID)
        {
            var words = File.ReadAllLines(@"words.txt").ToList();
            using (var connection = Util.GetOpenConnection())
            {
                var rnd = new Random();
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

        private static int InsertRandomlyNamedAuthorAndReturnID()
        {
            var firstnames = File.ReadAllLines(@"firstnames.txt").ToList();
            var surnames = File.ReadAllLines(@"surnames.txt").ToList();

            using (var connection = Util.GetOpenConnection())
            {
                using (var cmd = new SqlCommand("INSERT INTO [Authors](FirstName, LastName,EmailAddress) VALUES(@FirstName, @LastName, @EmailAddress); SELECT SCOPE_IDENTITY()", connection))
                {
                    var rnd = new Random();
                    string authorFirstname = firstnames[rnd.Next(firstnames.Count)];
                    string authorSurname = surnames[rnd.Next(surnames.Count)].CapitaliseFirstLetter();
                    string autorEmail = authorFirstname + "@" + authorSurname + ".com";
                    cmd.Parameters.AddWithValue("@FirstName", authorFirstname);
                    cmd.Parameters.AddWithValue("@LastName", authorSurname);
                    cmd.Parameters.AddWithValue("@EmailAddress", autorEmail);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
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
