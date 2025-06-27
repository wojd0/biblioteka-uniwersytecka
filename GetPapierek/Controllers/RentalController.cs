using GetPapierek.Models;
using GetPapierek.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GetPapierek.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RentalController : ControllerBase
    {
        private readonly IRentalRepository _wypozyczenieRepository;
        private readonly IBookRepository _ksiazkaRepository;

        public RentalController(
            IRentalRepository wypozyczenieRepository,
            IBookRepository ksiazkaRepository)
        {
            _wypozyczenieRepository = wypozyczenieRepository;
            _ksiazkaRepository = ksiazkaRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var wypozyczenia = await _wypozyczenieRepository.GetAllAsync();
            return Ok(wypozyczenia);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var wypozyczenie = await _wypozyczenieRepository.GetByIdAsync(id);
            if (wypozyczenie == null)
            {
                return NotFound($"Wypożyczenie o ID {id} nie zostało znalezione.");
            }
            return Ok(wypozyczenie);
        }

        [HttpGet("uzytkownik/{userId}")]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            var wypozyczenia = await _wypozyczenieRepository.GetByUserIdAsync(userId);
            return Ok(wypozyczenia);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Rental wypozyczenie)
        {
            if (wypozyczenie == null)
            {
                return BadRequest("Dane wypożyczenia są nieprawidłowe.");
            }

            var book = await _ksiazkaRepository.GetByIdAsync(wypozyczenie.BookId);
            if (book == null)
            {
                return NotFound($"Książka o ID {wypozyczenie.BookId} nie została znaleziona.");
            }

            var addedWypozyczenie = await _wypozyczenieRepository.AddAsync(wypozyczenie);
            return CreatedAtAction(nameof(GetById), new { id = addedWypozyczenie.RentalId }, addedWypozyczenie);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Rental wypozyczenie)
        {
            if (wypozyczenie == null || id != wypozyczenie.RentalId)
            {
                return BadRequest("Dane wypożyczenia są nieprawidłowe.");
            }

            var updatedWypozyczenie = await _wypozyczenieRepository.UpdateAsync(wypozyczenie);
            if (updatedWypozyczenie == null)
            {
                return NotFound($"Wypożyczenie o ID {id} nie zostało znalezione.");
            }
            return Ok(updatedWypozyczenie);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _wypozyczenieRepository.DeleteAsync(id);
            if (!result)
            {
                return NotFound($"Wypożyczenie o ID {id} nie zostało znalezione.");
            }
            return NoContent();
        }

        [HttpPost("{id}/zwrot")]
        public async Task<IActionResult> ReturnBook(int id)
        {
            var result = await _wypozyczenieRepository.ReturnBookAsync(id);
            if (!result)
            {
                return NotFound($"Wypożyczenie o ID {id} nie zostało znalezione lub książka została już zwrócona.");
            }
            return Ok();
        }
    }
}
