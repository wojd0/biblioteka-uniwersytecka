using GetPapierek.Data;
using GetPapierek.Models;
using GetPapierek.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GetPapierek.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly BibliotekDbContext _context;

        public BookRepository(BibliotekDbContext context)
        {
            _context = context;
        }

        public async Task<List<Book>> GetAllAsync()
        {
            return await _context.Ksiazki.Include(k => k.Category).ToListAsync();
        }

        public async Task<Book> GetByIdAsync(int id)
        {
            return await _context.Ksiazki
                .Include(k => k.Category)
                .FirstOrDefaultAsync(k => k.BookId == id);
        }

        public async Task<List<Book>> SearchAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return await GetAllAsync();

            query = query.ToLower();
            return await _context.Ksiazki
                .Include(k => k.Category)
                .Where(k => k.Title.ToLower().Contains(query) ||
                            k.Author.ToLower().Contains(query) ||
                            k.Category.NazwaKategorii.ToLower().Contains(query))
                .ToListAsync();
        }

        public async Task<Book> AddAsync(Book ksiazka)
        {
            await _context.Ksiazki.AddAsync(ksiazka);
            await _context.SaveChangesAsync();
            return ksiazka;
        }

        public async Task<Book> UpdateAsync(Book ksiazka)
        {
            var existingBook = await _context.Ksiazki.FindAsync(ksiazka.BookId);
            if (existingBook == null)
                return null;

            _context.Entry(existingBook).CurrentValues.SetValues(ksiazka);
            await _context.SaveChangesAsync();
            return existingBook;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var book = await _context.Ksiazki.FindAsync(id);
            if (book == null)
                return false;

            _context.Ksiazki.Remove(book);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
