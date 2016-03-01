using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace Four
{
    // driving me nuts not having tests for my code, and having to look at sql to see if the app has worked
    // didn't find problem with rnd producing the same numbers

    // Make fake Author and Book data from firstname, surname, and word
    // text files for testing BookTech website
    public class Bootstrap
    {
        static void MainFour()
        {
            //new AuthorAndBookGenerator().Run();
        }
    }
    

    [TestFixture]
    public class AuthorAndBookGenerator
    {
        //static Random rnd;
        //static List<string> firstnames;
        //static List<string> surnames;
        //static List<string> words;

        // how to test random numbers?
        // maybe just pass in a list of 1 firstname, and 1 surname
        // and test to make sure the app never generates the same author names?

        [Test]
        public void given_file_of_firstnames_should_load_into_a_list_of_strings()
        {
            List<string> firstnames = LoadFile(@"c:\temp\firstnames.txt");
            Assert.AreEqual(8608, firstnames.Count);
        }

        [Test]
        public void given_a_list_of_firstnames_and_surnames_should_return_a_fictional_author()
        {
            var firstnames = new List<string> {"Dave"};
            var surnames = new List<string> {"Mateer"};

            var result = GetRandomAuthor(firstnames, surnames);

            Assert.AreEqual("Dave Mateer", result);
        }

        [Test]
        public void given_a_list_of_firstnames_and_surnames_should_return_a_fictional_author_capitalised_both_names()
        {
            var firstnames = new List<string> {"dave"};
            var surnames = new List<string> {"mateer"};

            var result = GetRandomAuthor(firstnames, surnames);

            Assert.AreEqual("Dave Mateer", result);
        }

        [Test]
        public void given_a_list_of_words_should_return_a_book_title()
        {
            var words = new List<string> {"hello", "world"};

            var result = GetBookTitle(words);

            // as it is random, could be in two different orders
            string first = "Hello of the World";
            string second = "World of the Hello";

            // assert either of the above is true
            bool thing = result == first || result == second;
            Assert.IsTrue(thing);

        }

        private string GetBookTitle(List<string> words)
        {
            var rnd = new Random();
            int firstWordIndex = rnd.Next(words.Count);
            string firstWord = words[firstWordIndex].CapitaliseFirstLetter();
            words.RemoveAt(firstWordIndex);

            int secondWordIndex = rnd.Next(words.Count);
            string secondWord = words[secondWordIndex].CapitaliseFirstLetter();

            return firstWord + " of the " + secondWord;
        }


        private string GetRandomAuthor(List<string> firstnames, List<string> surnames)
        {
            var rnd = new Random();
            string firstname = firstnames[rnd.Next(firstnames.Count)].CapitaliseFirstLetter();
            string surname = surnames[rnd.Next(surnames.Count)].CapitaliseFirstLetter();
            return firstname + " " + surname;
        }

        private List<string> LoadFile(string fileName)
        {
            return File.ReadAllLines(fileName).ToList();
        }


        //public void Run()
        //{
        //    rnd = new Random();
        //    firstnames = LoadFile("firstnames.txt");
        //    surnames = LoadFile("surnames.txt");
        //    words = LoadFile("words.txt");

        //    for (int i = 0; i < 5; i++)
        //    {
        //        var authorID = InsertRandomlyNamedAuthorAndReturnID();
        //        AssignRandomNumberOfBookTitlesToAuthor(authorID);
        //    }
        //}

        //private int InsertRandomlyNamedAuthorAndReturnID()
        //{
        //    using (var connection = Util.GetOpenConnection())
        //    {
        //        using (var cmd = new SqlCommand("INSERT INTO [Authors](FirstName, LastName,EmailAddress) VALUES(@FirstName, @LastName, @EmailAddress); SELECT SCOPE_IDENTITY()", connection))
        //        {
        //            string authorFirstname = firstnames[rnd.Next(firstnames.Count)];
        //            string authorSurname = surnames[rnd.Next(surnames.Count)].CapitaliseFirstLetter();
        //            string autorEmail = authorFirstname + "@" + authorSurname + ".com";
        //            cmd.Parameters.AddWithValue("@FirstName", authorFirstname);
        //            cmd.Parameters.AddWithValue("@LastName", authorSurname);
        //            cmd.Parameters.AddWithValue("@EmailAddress", autorEmail);
        //            return Convert.ToInt32(cmd.ExecuteScalar());
        //        }
        //    }
        //}

        //private void AssignRandomNumberOfBookTitlesToAuthor(int authorID)
        //{
        //    using (var connection = Util.GetOpenConnection())
        //    {
        //        for (int j = 0; j < rnd.Next(1, 7); j++)
        //        {
        //            using (var cmd = new SqlCommand("INSERT INTO Books(Title, AuthorID) VALUES(@Title, @AuthorID)", connection))
        //            {
        //                var bookTitle = GenerateBookTitle();
        //                cmd.Parameters.AddWithValue("@Title", bookTitle);
        //                cmd.Parameters.AddWithValue("@AuthorID", authorID);
        //                cmd.ExecuteScalar();
        //            }
        //        }
        //    }
        //}

        //private string GenerateBookTitle()
        //{
        //    string firstWord = words[rnd.Next(words.Count)].CapitaliseFirstLetter();
        //    string secondWord = words[rnd.Next(words.Count)].CapitaliseFirstLetter();
        //    string bookTitle = firstWord + " of the " + secondWord;
        //    return bookTitle;
        //}
    }
}
