using Microsoft.EntityFrameworkCore;
using WebApplication1.Abstracts;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class BookService : IBookService
    {
        private readonly ApplicationDbContext _context;

        public BookService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Book?> getBookByName(string name)
        {
            return await _context.Books
                .Include(b => b.Category)
                .FirstOrDefaultAsync(x => x.Title.ToLower() == name.ToLower() && !x.IsDeleted);
        }

        public async Task<Book?> getBookById(int id)
        {
            return await _context.Books
                .Include(b => b.Category)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        public async Task<Book> addBook(Book book)
        {
            var existingBook = await _context.Books.FirstOrDefaultAsync(x => x.ISBN == book.ISBN);
            if (existingBook == null)
            {
                await _context.AddAsync(book);
                await _context.SaveChangesAsync();
                return book;
            }
            throw new Exception("A book with this ISBN already exists");
        }

        public async Task<Book> rentBook(int bookId, string userId)
        {
            var book = await _context.Books
                .Include(b => b.Category)
                .FirstOrDefaultAsync(x => x.Id == bookId && !x.IsDeleted && x.Available);
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            
            if (book != null && user != null)
            {
                var rental = new Rental
                {
                    BookId = book.Id,
                    ISBN = book.ISBN,
                    UserId = userId,
                    RentalDate = DateTime.UtcNow,
                    ReturnDate = DateTime.UtcNow.AddDays(7),
                    IsReturned = false
                };

                book.Available = false;
                await _context.Rentals.AddAsync(rental); // add rental first to prevent conflict with the book
                _context.Books.Update(book);
                await _context.SaveChangesAsync();
                return book;
            }
            throw new Exception("Book not available or user not found");
        }

        public async Task<IEnumerable<Book>> getAllBooks()
        {
            return await _context.Books
                .Include(b => b.Category)
                .Where(b => b.Available && !b.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetFilteredBooks(string? titleFilter, string? categoryFilter)
        {
            var query = _context.Books
                .Include(b => b.Category)
                .Where(b => !b.IsDeleted && b.Available)
                .AsQueryable();

            if (!string.IsNullOrEmpty(titleFilter))
            {
                query = query.Where(b => EF.Functions.Like(b.Title.ToLower(), $"%{titleFilter.ToLower()}%"));
            }

            if (!string.IsNullOrEmpty(categoryFilter) && categoryFilter != "All")
            {
                query = query.Where(b => b.Category.Name == categoryFilter);
            }

            return await query.ToListAsync();
        }

        public async Task<List<Category>> GetDistinctCategories()
        {
            return await _context.Categories
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<bool> removeBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                book.IsDeleted = true;
                _context.Books.Update(book);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<Book> updateBook(Book book)
        {
            try
            {
                var existingBook = await _context.Books
                    .Include(b => b.Category)
                    .FirstOrDefaultAsync(x => x.Id == book.Id);

                if (existingBook == null)
                {
                    throw new Exception("Book not found");
                }

                existingBook.ISBN = book.ISBN;
                existingBook.Title = book.Title;
                existingBook.Author = book.Author;
                existingBook.Description = book.Description;
                existingBook.ImageUrl = book.ImageUrl;
                existingBook.CategoryId = book.CategoryId;
                existingBook.Available = book.Available;

                await _context.SaveChangesAsync();

                return await _context.Books
                    .Include(b => b.Category)
                    .FirstAsync(b => b.Id == existingBook.Id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating book: {ex.Message}");
            }
        }

        private async Task<bool> isExist(int id)
        {
            return await _context.Books.AnyAsync(x => x.Id == id && !x.IsDeleted);
        }
    }
}
