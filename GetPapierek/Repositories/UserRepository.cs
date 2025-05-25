using GetPapierek.Data;
using GetPapierek.Models;
using GetPapierek.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GetPapierek.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly BibliotekDbContext _context;

        public UserRepository(BibliotekDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Uzytkownicy.ToListAsync();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.Uzytkownicy.FindAsync(id);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Uzytkownicy.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> AddAsync(User uzytkownik)
        {
            // In a real application, we would hash the password here
            // using something like BCrypt before saving

            await _context.Uzytkownicy.AddAsync(uzytkownik);
            await _context.SaveChangesAsync();
            return uzytkownik;
        }

        public async Task<User> UpdateAsync(User uzytkownik)
        {
            var existingUser = await _context.Uzytkownicy.FindAsync(uzytkownik.UserId);
            if (existingUser == null)
                return null;

            // In a real application, check if password changed and hash it if needed

            _context.Entry(existingUser).CurrentValues.SetValues(uzytkownik);
            await _context.SaveChangesAsync();
            return existingUser;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var uzytkownik = await _context.Uzytkownicy.FindAsync(id);
            if (uzytkownik == null)
                return false;

            _context.Uzytkownicy.Remove(uzytkownik);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<User> AuthenticateAsync(string email, string haslo)
        {
            // In a real application, we would hash the password and compare hashes
            var uzytkownik = await _context.Uzytkownicy
                .FirstOrDefaultAsync(u => u.Email == email && u.Password == haslo);

            return uzytkownik;
        }
    }
}
