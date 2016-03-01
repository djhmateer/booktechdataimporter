using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Four
{
    public class AuthorAndBookGenerator
    {
        public void Run()
        {
            var firstnames = Util.LoadFile("firstnames.txt");
            var surnames = Util.LoadFile("surnames.txt");
            
            for (int i = 0; i < 5; i++)
            {
                Author author = GetRandomAuthor(firstnames, surnames);

                author = GiveAuthorRandomNumberOfBooks(author);

                WriteAuthorWithBooksToDb(author);
            }
        }

        public Author GetRandomAuthor(List<string> firstnames, List<string> surnames)
        {
            var rnd = new Random();
            string firstname = firstnames[rnd.Next(firstnames.Count)].CapitaliseFirstLetter();
            string surname = surnames[rnd.Next(surnames.Count)].CapitaliseFirstLetter();
            return new Author { Firstname = firstname, Surname = surname };
        }

        public Author GiveAuthorRandomNumberOfBooks(Author author)
        {
            var words = Util.LoadFile("words.txt");
            var books = new List<Book>();
            // Give author a random number of books
            for (int j = 0; j < new Random().Next(1, 5); j++)
            {
                Book book = GetBookTitle(words);
                books.Add(book);
            }
            author.Books = books;
            return author;
        }

        public Book GetBookTitle(List<string> words)
        {
            var rnd = new Random();
            int firstWordIndex = rnd.Next(words.Count);
            string firstWord = words[firstWordIndex].CapitaliseFirstLetter();
            words.RemoveAt(firstWordIndex);

            int secondWordIndex = rnd.Next(words.Count);
            string secondWord = words[secondWordIndex].CapitaliseFirstLetter();

            Book book = new Book { Title = firstWord + " of the " + secondWord };
            return book;
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
                    string autorEmail = authorFirstname + "@" + authorSurname + ".com";
                    cmd.Parameters.AddWithValue("@FirstName", authorFirstname);
                    cmd.Parameters.AddWithValue("@LastName", authorSurname);
                    cmd.Parameters.AddWithValue("@EmailAddress", autorEmail);
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
