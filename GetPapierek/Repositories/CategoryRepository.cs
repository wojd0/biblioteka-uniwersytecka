using GetPapierek.Data;
using GetPapierek.Models;
using GetPapierek.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GetPapierek.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly BibliotekDbContext _context;

        public CategoryRepository(BibliotekDbContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _context.KategorieKsiazek.ToListAsync();
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await _context.KategorieKsiazek.FindAsync(id);
        }

        public async Task<Category> AddAsync(Category kategoria)
        {
            await _context.KategorieKsiazek.AddAsync(kategoria);
            await _context.SaveChangesAsync();
            return kategoria;
        }

        public async Task<Category> UpdateAsync(Category kategoria)
        {
            var existingKategoria = await _context.KategorieKsiazek.FindAsync(kategoria.IdKategorii);
            if (existingKategoria == null)
                return null;

            _context.Entry(existingKategoria).CurrentValues.SetValues(kategoria);
            await _context.SaveChangesAsync();
            return existingKategoria;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var kategoria = await _context.KategorieKsiazek.FindAsync(id);
            if (kategoria == null)
                return false;

            _context.KategorieKsiazek.Remove(kategoria);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
