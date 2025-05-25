using GetPapierek.Models;

namespace GetPapierek.Repositories.Interfaces
{
    public interface IRentalRepository
    {
        Task<List<Rental>> GetAllAsync();
        Task<List<Rental>> GetByUserIdAsync(int userId);
        Task<Rental> GetByIdAsync(int id);
        Task<Rental> AddAsync(Rental wypozyczenie);
        Task<Rental> UpdateAsync(Rental wypozyczenie);
        Task<bool> DeleteAsync(int id);
        Task<bool> ReturnBookAsync(int id);
    }
}
