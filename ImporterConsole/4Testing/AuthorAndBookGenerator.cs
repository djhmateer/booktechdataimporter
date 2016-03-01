using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace Four
{
    public class AuthorAndBookGenerator
    {
        public void Run()
        {
            var firstnames = File.ReadAllLines("firstnames.txt").ToList(); 
            var surnames    = File.ReadAllLines("surnames.txt").ToList();
            var words = File.ReadAllLines("words.txt").ToList();

            for (int i = 0; i < 5; i++)
            {
                Author author = GetRandomAuthor(firstnames, surnames);
                Author authorWithBooks = GiveAuthorRandomNumberOfBooks(words, author);
                WriteAuthorWithBooksToDb(authorWithBooks);
            }
        }

        public Author GetRandomAuthor(List<string> firstnames, List<string> surnames)
        {
            var r = GetRandom();
            string firstname = firstnames[r.Next(firstnames.Count)].CapitaliseFirstLetter();
            string surname = surnames[r.Next(surnames.Count)].CapitaliseFirstLetter();
            return new Author { Firstname = firstname, Surname = surname };
        }

        public Author GiveAuthorRandomNumberOfBooks(List<string> words, Author author)
        {
            var books = new List<Book>();
            // Give author a random number of books
            for (int j = 0; j < GetRandom().Next(1, 5); j++)
            {
                Book book = GetBookTitle(words);
                books.Add(book);
            }
            author.Books = books;
            return author;
        }

        public Book GetBookTitle(List<string> words)
        {
            var r = GetRandom();
            int firstWordIndex = r.Next(words.Count);
            string firstWord = words[firstWordIndex].CapitaliseFirstLetter();
            words.RemoveAt(firstWordIndex);

            int secondWordIndex = r.Next(words.Count);
            string secondWord = words[secondWordIndex].CapitaliseFirstLetter();

            var book = new Book { Title = firstWord + " of the " + secondWord };
            return book;
        }

        // To stop similar random numbers use the same Random instance 
        private static Random rnd;
        private static Random GetRandom()
        {
            if (rnd != null) return rnd;
            rnd = new Random();
            return rnd;
        }

        public void WriteAuthorWithBooksToDb(Author authorWithBooks)
        {
            using (var connection = Util.GetOpenConnection())
            {
                int authorID;
                using (var cmd = new SqlCommand("INSERT INTO [Authors](FirstName, LastName,EmailAddress) VALUES(@FirstName, @LastName, @EmailAddress); SELECT SCOPE_IDENTITY()", connection))
                {
                    string authorFirstname = authorWithBooks.Firstname;
                    string authorSurname = authorWithBooks.Surname;
                    string authorEmail = authorFirstname + "@" + authorSurname + ".com";
                    cmd.Parameters.AddWithValue("@FirstName", authorFirstname);
                    cmd.Parameters.AddWithValue("@LastName", authorSurname);
                    cmd.Parameters.AddWithValue("@EmailAddress", authorEmail);
                    authorID = Convert.ToInt32(cmd.ExecuteScalar());
                }

                // write books
                foreach (var book in authorWithBooks.Books)
                {
                    using (var cmd = new SqlCommand("INSERT INTO Books(Title, AuthorID) VALUES(@Title, @AuthorID)", connection))
                    {
                        cmd.Parameters.AddWithValue("@Title", book.Title);
                        cmd.Parameters.AddWithValue("@AuthorID", authorID);
                        cmd.ExecuteScalar();
                    }
                }
            }
        }
    }
}
