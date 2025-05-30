using GetPapierek.Data;
using GetPapierek.Models;
using GetPapierek.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GetPapierek.Repositories
{
    public class RentalRepository : IRentalRepository
    {
        private readonly LibraryDbContext _context;

        public RentalRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<List<Rental>> GetAllAsync()
        {
            return await _context.Rentals
                .Include(w => w.User)
                .Include(w => w.Book)
                .ThenInclude(k => k!.Category)
                .ToListAsync();
        }

        public async Task<List<Rental>> GetByUserIdAsync(int userId)
        {
            return await _context.Rentals
                .Include(w => w.User)
                .Include(w => w.Book)
                .ThenInclude(k => k!.Category)
                .Where(w => w.UserId == userId)
                .ToListAsync();
        }

        public async Task<Rental?> GetByIdAsync(int id)
        {
            return await _context.Rentals
                .Include(w => w.User)
                .Include(w => w.Book)
                .ThenInclude(k => k!.Category)
                .FirstOrDefaultAsync(w => w.RentalId == id);
        }

        public async Task<Rental> AddAsync(Rental rental)
        {
            if (rental.UserId != 0)
            {
                var existingUser = await _context.Users.FindAsync(rental.UserId);
                if (existingUser != null)
                {
                    rental.User = existingUser;
                }
            }
            if (rental.BookId != 0)
            {
                var existingBook = await _context.Books.FindAsync(rental.BookId);
                if (existingBook != null)
                {
                    rental.Book = existingBook;
                }
            }
            rental.RentalDate = DateTime.Now;
            rental.Status = RentalStatus.Rented;

            await _context.Rentals.AddAsync(rental);
            await _context.SaveChangesAsync();
            return rental;
        }

        public async Task<Rental?> UpdateAsync(Rental rental)
        {
            var existingRental = await _context.Rentals.FindAsync(rental.RentalId);
            if (existingRental == null)
                return null;

            _context.Entry(existingRental).CurrentValues.SetValues(rental);
            await _context.SaveChangesAsync();
            return existingRental;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var rental = await _context.Rentals.FindAsync(id);
            if (rental == null)
                return false;

            _context.Rentals.Remove(rental);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ReturnBookAsync(int id)
        {
            var rental = await _context.Rentals.FindAsync(id);
            if (rental == null || rental.Status == RentalStatus.Returned)
                return false;

            rental.Status = RentalStatus.Returned;
            rental.ReturnDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
