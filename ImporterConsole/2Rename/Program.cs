// Refactored code after 1 - Simple Checks
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

// Make fake Author and Book data from firstname, surname, and word
// text files for testing BookTech website
namespace TwoRename
{
    class Program
    {
        static void MainTwoRename()
        {
            var fns = File.ReadAllLines(@"firstnames.txt").ToList();
            var sns = File.ReadAllLines(@"surnames.txt").ToList();
            var wrds = File.ReadAllLines(@"words.txt").ToList();
            Console.WriteLine(fns.Count + " firstnames loaded");
            Console.WriteLine(sns.Count + " surnames loaded");
            Console.WriteLine(wrds.Count + " book title words loaded");
            var rnd = new Random();
            var st = ConfigurationManager.ConnectionStrings["BookTechConnectionString"].ConnectionString;
            using (var sqlServerDBConnectionFactory = new SqlConnection(st))
            {
                sqlServerDBConnectionFactory.Open();
                // Insert Authors
                for (int authorCounter = 0; authorCounter < 5; authorCounter++)
                {
                    int authorID;
                    using (var cmd = new SqlCommand("INSERT INTO [Authors](FirstName, LastName,EmailAddress) VALUES(@FirstName, @LastName, @EmailAddress); SELECT SCOPE_IDENTITY()", sqlServerDBConnectionFactory))
                    {
                        string fn = fns[rnd.Next(fns.Count)];
                        string sn = sns[rnd.Next(sns.Count)].CapitaliseFirstLetter();
                        string em = fn + "@" + sn + ".com";
                        cmd.Parameters.AddWithValue("@FirstName", fn);
                        cmd.Parameters.AddWithValue("@LastName", sn);
                        cmd.Parameters.AddWithValue("@EmailAddress", em);
                        authorID = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    // Add random number of books to the newly created Author
                    for (int bookCounter = 0; bookCounter < rnd.Next(1, 7); bookCounter++)
                    {
                        using (var cmd = new SqlCommand("INSERT INTO Books(Title, AuthorID) VALUES(@Title, @AuthorID)", sqlServerDBConnectionFactory))
                        {
                            string fw = wrds[rnd.Next(wrds.Count)].CapitaliseFirstLetter();
                            string sw = wrds[rnd.Next(wrds.Count)].CapitaliseFirstLetter();
                            string bt = fw + " of the " + sw;
                            cmd.Parameters.AddWithValue("@Title", bt);
                            cmd.Parameters.AddWithValue("@AuthorID", authorID);
                            cmd.ExecuteScalar();
                        }
                    }
                }
            }
        }
    }
}