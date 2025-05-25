using GetPapierek.Models;

namespace GetPapierek.Repositories.Interfaces
{
    public interface IBookRepository
    {
        Task<List<Book>> GetAllAsync();
        Task<Book> GetByIdAsync(int id);
        Task<List<Book>> SearchAsync(string query);
        Task<Book> AddAsync(Book ksiazka);
        Task<Book> UpdateAsync(Book ksiazka);
        Task<bool> DeleteAsync(int id);
    }
}
