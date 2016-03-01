using System.Collections.Generic;

namespace Four
{
    public class Author
    {
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public List<Book> Books { get; set; }
    }
}