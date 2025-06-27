using GetPapierek.Data;
using GetPapierek.Models;
using GetPapierek.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using GetPapierek.Utils;

namespace GetPapierek.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly LibraryDbContext _context;

        public UserRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> AddAsync(User user)
        {
            if (!string.IsNullOrEmpty(user.Password))
            {
                user.Password = PasswordHasher.HashPassword(user.Password);
            }
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> UpdateAsync(User user)
        {
            var existingUser = await _context.Users.FindAsync(user.UserId);
            if (existingUser == null)
                return null;

            if (!string.IsNullOrEmpty(user.Password) && user.Password != existingUser.Password)
            {
                user.Password = PasswordHasher.HashPassword(user.Password);
            }
            _context.Entry(existingUser).CurrentValues.SetValues(user);
            await _context.SaveChangesAsync();
            return existingUser;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<User?> AuthenticateAsync(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return null;
            if (!PasswordHasher.VerifyPassword(password, user.Password)) return null;
            return user;
        }
    }
}
