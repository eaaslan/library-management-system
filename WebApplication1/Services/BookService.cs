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

        public Task<Book> getBookByName(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<Book> getBookById(string id)
        {
            //todo kontrol et
            var book =  await _context.Books.FirstOrDefaultAsync(x => x.Id == id && !x.isDeleted);
            return book;
        }

        public async Task<Book> addBook(Book book)
        {

            if (!isExist(book.Id))
            {

                await _context.AddAsync(book);
                await _context.SaveChangesAsync();
                return book;
            }
            throw new Exception("The book already exist");
           
        }

        public async Task<Book> rentBook(string bookId,string userId)
        {
            var book = await _context.Books.FirstOrDefaultAsync(x => x.Id == bookId && !x.isDeleted && x.Available);
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (isExist(book.Id) && user!=null)
            {

                var rental = new Rental()
                {
                    Book = book,
                    User = user,
                    RentalDate = DateTime.UtcNow,
                    ReturnDate = DateTime.UtcNow.AddDays(7),
                    BookISBN = book.Id,
                    IsReturned = false,
                    UserId = userId,
                    Id = Guid.NewGuid().ToString()
                };

                book.Available = false;
                _context.Rentals.Add(rental);
                await _context.SaveChangesAsync();
                _context.Books.Update(book);
                await _context.SaveChangesAsync();
                return book;
            }
            //todo throw exception
            throw new Exception("The book already exist");

        }

        public async Task<IEnumerable<Book>> getAllBooks()
        {

          var books = await _context.Books.Where(b => b.Available && !b.isDeleted).ToListAsync();

            return books;
        }

        public  async Task<bool> removeBook(string id)
        {
            var book = await _context.Books.FirstOrDefaultAsync(x=>x.Id.Equals(id));
            
            if (book != null) {
                book.isDeleted = true;
                _context.Books.Update(book);
                await _context.SaveChangesAsync();
                return true;

            }

            return false;
        }

        public async Task<Book> updateBook(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
            return book;


        }

        public bool isExist(string id) { 
            
            var bookId = id.ToLower();
            var state=_context.Books.Any(x=>x.Id.ToLower() == bookId);

            return state;
        
        }
    }
}
