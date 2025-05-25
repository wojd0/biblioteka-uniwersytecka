using GetPapierek.Models;
using GetPapierek.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GetPapierek.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;

        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var books = await _bookRepository.GetAllAsync();
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
            {
                return NotFound($"Book with ID {id} was not found.");
            }
            return Ok(book);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string query)
        {
            var books = await _bookRepository.SearchAsync(query);
            return Ok(books);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Book book)
        {
            if (book == null)
            {
                return BadRequest("Book data is invalid.");
            }

            var addedBook = await _bookRepository.AddAsync(book);
            return CreatedAtAction(nameof(GetById), new { id = addedBook.BookId }, addedBook);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Book book)
        {
            if (book == null || id != book.BookId)
            {
                return BadRequest("Book data is invalid.");
            }

            var updatedBook = await _bookRepository.UpdateAsync(book);
            if (updatedBook == null)
            {
                return NotFound($"Book with ID {id} was not found.");
            }
            return Ok(updatedBook);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _bookRepository.DeleteAsync(id);
            if (!result)
            {
                return NotFound($"Book with ID {id} was not found.");
            }
            return NoContent();
        }
    }
}
