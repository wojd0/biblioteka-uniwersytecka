using GetPapierek.Data;
using GetPapierek.Models;
using GetPapierek.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GetPapierek.Repositories
{
    public class RentalRepository : IRentalRepository
    {
        private readonly BibliotekDbContext _context;

        public RentalRepository(BibliotekDbContext context)
        {
            _context = context;
        }

        public async Task<List<Rental>> GetAllAsync()
        {
            return await _context.Wypozyczenia
                .Include(w => w.User)
                .Include(w => w.Book)
                .ThenInclude(k => k.Category)
                .ToListAsync();
        }

        public async Task<List<Rental>> GetByUserIdAsync(int userId)
        {
            return await _context.Wypozyczenia
                .Include(w => w.User)
                .Include(w => w.Book)
                .ThenInclude(k => k.Category)
                .Where(w => w.UserId == userId)
                .ToListAsync();
        }

        public async Task<Rental> GetByIdAsync(int id)
        {
            return await _context.Wypozyczenia
                .Include(w => w.User)
                .Include(w => w.Book)
                .ThenInclude(k => k.Category)
                .FirstOrDefaultAsync(w => w.RentalId == id);
        }

        public async Task<Rental> AddAsync(Rental wypozyczenie)
        {
            // Set default values for new loan
            wypozyczenie.RentalDate = DateTime.Now;
            wypozyczenie.Status = RentalStatus.Rented;

            await _context.Wypozyczenia.AddAsync(wypozyczenie);
            await _context.SaveChangesAsync();
            return wypozyczenie;
        }

        public async Task<Rental> UpdateAsync(Rental wypozyczenie)
        {
            var existingWypozyczenie = await _context.Wypozyczenia.FindAsync(wypozyczenie.RentalId);
            if (existingWypozyczenie == null)
                return null;

            _context.Entry(existingWypozyczenie).CurrentValues.SetValues(wypozyczenie);
            await _context.SaveChangesAsync();
            return existingWypozyczenie;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var wypozyczenie = await _context.Wypozyczenia.FindAsync(id);
            if (wypozyczenie == null)
                return false;

            _context.Wypozyczenia.Remove(wypozyczenie);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ReturnBookAsync(int id)
        {
            var wypozyczenie = await _context.Wypozyczenia.FindAsync(id);
            if (wypozyczenie == null || wypozyczenie.Status == RentalStatus.Returned)
                return false;

            wypozyczenie.Status = RentalStatus.Returned;
            wypozyczenie.ReturnDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
