using System.Collections.Generic;
using NUnit.Framework;

namespace Four
{
    [TestFixture]
    public class AuthorAndBookGeneratorTests
    {
        private AuthorAndBookGenerator abg = new AuthorAndBookGenerator();

        [Test]
        public void given_file_of_firstnames_should_load_into_a_list_of_strings()
        {
            List<string> firstnames = Util.LoadFile(@"c:\temp\firstnames.txt");
            Assert.AreEqual(8608, firstnames.Count);
        }

        [Test]
        public void given_a_list_of_firstnames_and_surnames_should_return_a_fictional_author()
        {
            var firstnames = new List<string> { "Dave" };
            var surnames = new List<string> { "Mateer" };

            Author result = abg.GetRandomAuthor(firstnames, surnames);

            Assert.AreEqual("Dave", result.Firstname);
            Assert.AreEqual("Mateer", result.Surname);
        }

        [Test]
        public void given_a_list_of_firstnames_and_surnames_should_return_a_fictional_author_capitalised_both_names()
        {
            var firstnames = new List<string> { "dave" };
            var surnames = new List<string> { "mateer" };

            Author result = abg.GetRandomAuthor(firstnames, surnames);

            Assert.AreEqual("Dave", result.Firstname);
            Assert.AreEqual("Mateer", result.Surname);
        }

        [Test]
        public void given_a_list_of_words_should_return_a_book_title()
        {
            var words = new List<string> { "hello", "world" };

            Book book = abg.GetBookTitle(words);

            // as it is random, could be in two different orders
            string first = "Hello of the World";
            string second = "World of the Hello";

            // assert either of the above is true
            bool thing = book.Title == first || book.Title == second;
            Assert.IsTrue(thing);
        }

        [Test]
        public void given_an_author_and_2_books_should_save_to_the_db()
        {
            var books = new List<Book> { new Book { Title = "Dummy title1" }, new Book { Title = "Dummy title2" } };
            var authorWithBooks = new Author { Firstname = "Dave", Surname = "Mateer", Books = books };

            abg.WriteAuthorWithBooksToDb(authorWithBooks);
        }
    }
}