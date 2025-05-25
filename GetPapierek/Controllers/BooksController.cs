using GetPapierek.Models;
using GetPapierek.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GetPapierek.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _ksiazkaRepository;

        public BooksController(IBookRepository ksiazkaRepository)
        {
            _ksiazkaRepository = ksiazkaRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var books = await _ksiazkaRepository.GetAllAsync();
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var book = await _ksiazkaRepository.GetByIdAsync(id);
            if (book == null)
            {
                return NotFound($"Książka o ID {id} nie została znaleziona.");
            }
            return Ok(book);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string query)
        {
            var books = await _ksiazkaRepository.SearchAsync(query);
            return Ok(books);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Book book)
        {
            if (book == null)
            {
                return BadRequest("Dane książki są nieprawidłowe.");
            }

            var addedBook = await _ksiazkaRepository.AddAsync(book);
            return CreatedAtAction(nameof(GetById), new { id = addedBook.IdKsiazki }, addedBook);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Book book)
        {
            if (book == null || id != book.IdKsiazki)
            {
                return BadRequest("Dane książki są nieprawidłowe.");
            }

            var updatedBook = await _ksiazkaRepository.UpdateAsync(book);
            if (updatedBook == null)
            {
                return NotFound($"Książka o ID {id} nie została znaleziona.");
            }
            return Ok(updatedBook);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _ksiazkaRepository.DeleteAsync(id);
            if (!result)
            {
                return NotFound($"Książka o ID {id} nie została znaleziona.");
            }
            return NoContent();
        }
    }
}
