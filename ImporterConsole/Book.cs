//namespace ImporterConsole
//{
//    public class Book
//    {
//        // Property (expose Fields)
//        public string Title { get; set; }

//        // Field (should almost alwyas be kept private) 
//        // private property / private variable
//        private string _title;

//        // Constuctor
//        public Book(){}

//    }

//    // C# Language definitions helper

//    // BookRepository implements IBookRepository
//    public class BookRepository : IBookRepository
//    {
//        private readonly ILogger _log;

//        // Pure (Poor mans) constructor dependency injection
//        public BookRepository(ILogger log)
//        {
//            _log = log;
//        }

//        // Book is a parameter (it is passed as an argument - value)
//        public void SaveBook(Book book)
//        {
//            _log.Debug("Saving book");
//        }
//    }

//    public interface ILogger { void Debug(string message); }

//    public interface IBookRepository { }

//    // BookController is a BaseController (inheritance)
//    public class BookController : BaseController
//    {
//        // Field
//        // BookController has a BookRepository (composition)
//        private readonly IBookRepository _repo;


//        public BookController(IBookRepository repo)
//        {
//            _repo = repo;
//        }

//        public ViewResult BookEdit(Book book)
//        {
//            // Passing an argument (the value)
//            _repo.SaveBook(book);
//        }
//    }

//    public class BaseController { }
//}