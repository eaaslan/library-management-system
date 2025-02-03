using WebApplication1.Models;

namespace WebApplication1.Abstracts
{

    public interface IBookService
    {

        Task<IEnumerable<Book>> getAllBooks();

        Task<Book> getBookById(string id);
        Task<Book> getBookByName(string name);

        Task<Book> addBook(Book book);

        Task<bool> removeBook(string id);

        Task<Book> updateBook(Book book);

        Task<Book> rentBook(string bookId, string userId);


    }
}
