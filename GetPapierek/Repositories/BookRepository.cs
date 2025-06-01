using GetPapierek.Data;
using GetPapierek.Models;
using GetPapierek.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GetPapierek.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly LibraryDbContext _context;

        public BookRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<List<Book>> GetAllAsync()
        {
            return await _context.Books.Include(k => k.Category).ToListAsync();
        }

        public async Task<Book?> GetByIdAsync(int id)
        {
            return await _context.Books
                .Include(k => k.Category)
                .FirstOrDefaultAsync(k => k.BookId == id);
        }

        public async Task<List<Book>> SearchAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return await GetAllAsync();

            query = query.ToLower();
            return await _context.Books
                .Include(k => k.Category)
                .Where(k => k.Title.ToLower().Contains(query) ||
                            k.Author.ToLower().Contains(query) ||
                            (k.Category != null && k.Category.Name.ToLower().Contains(query)))
                .ToListAsync();
        }

        public async Task<Book> AddAsync(Book book)
        {
            if (book.CategoryId.HasValue)
            {
                var existingCategory = await _context.Categories.FindAsync(book.CategoryId.Value);
                if (existingCategory != null)
                {
                    book.Category = existingCategory;
                }
            }
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<Book?> UpdateAsync(Book book)
        {
            var existingBook = await _context.Books.FindAsync(book.BookId);
            if (existingBook == null)
                return null;

            _context.Entry(existingBook).CurrentValues.SetValues(book);
            await _context.SaveChangesAsync();
            return existingBook;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
                return false;

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
