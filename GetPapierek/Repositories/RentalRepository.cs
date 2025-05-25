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
                .Include(w => w.Uzytkownik)
                .Include(w => w.Ksiazka)
                .ThenInclude(k => k.Kategoria)
                .ToListAsync();
        }

        public async Task<List<Rental>> GetByUserIdAsync(int userId)
        {
            return await _context.Wypozyczenia
                .Include(w => w.Uzytkownik)
                .Include(w => w.Ksiazka)
                .ThenInclude(k => k.Kategoria)
                .Where(w => w.IdUzytkownika == userId)
                .ToListAsync();
        }

        public async Task<Rental> GetByIdAsync(int id)
        {
            return await _context.Wypozyczenia
                .Include(w => w.Uzytkownik)
                .Include(w => w.Ksiazka)
                .ThenInclude(k => k.Kategoria)
                .FirstOrDefaultAsync(w => w.IdWypozyczenia == id);
        }

        public async Task<Rental> AddAsync(Rental wypozyczenie)
        {
            // Set default values for new loan
            wypozyczenie.DataWypozyczenia = DateTime.Now;
            wypozyczenie.Status = RentalStatus.Wypozyczona;

            await _context.Wypozyczenia.AddAsync(wypozyczenie);
            await _context.SaveChangesAsync();
            return wypozyczenie;
        }

        public async Task<Rental> UpdateAsync(Rental wypozyczenie)
        {
            var existingWypozyczenie = await _context.Wypozyczenia.FindAsync(wypozyczenie.IdWypozyczenia);
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
            if (wypozyczenie == null || wypozyczenie.Status == RentalStatus.Zwrocona)
                return false;

            wypozyczenie.Status = RentalStatus.Zwrocona;
            wypozyczenie.DataZwrotu = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
