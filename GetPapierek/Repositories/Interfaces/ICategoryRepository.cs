using GetPapierek.Models;

namespace GetPapierek.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllAsync();
        Task<Category> GetByIdAsync(int id);
        Task<Category> AddAsync(Category kategoria);
        Task<Category> UpdateAsync(Category kategoria);
        Task<bool> DeleteAsync(int id);
    }
}
