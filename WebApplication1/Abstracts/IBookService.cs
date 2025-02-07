using WebApplication1.Models;

namespace WebApplication1.Abstracts
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> getAllBooks();

        Task<Book?> getBookById(int id);
        Task<Book?> getBookByName(string name);

        Task<Book> addBook(Book book);

        Task<bool> removeBook(int id);

        Task<Book> updateBook(Book book);

        Task<Book> rentBook(int bookId, string userId);

        Task<List<Category>> GetDistinctCategories();

        Task<IEnumerable<Book>> GetFilteredBooks(string? titleFilter, string? categoryFilter); //todo: add author filter    
    }
}
