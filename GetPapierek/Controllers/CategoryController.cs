using GetPapierek.Models;
using GetPapierek.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GetPapierek.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _kategoriaRepository;

        public CategoryController(ICategoryRepository kategoriaRepository)
        {
            _kategoriaRepository = kategoriaRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _kategoriaRepository.GetAllAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _kategoriaRepository.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound($"Kategoria o ID {id} nie została znaleziona.");
            }
            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Category category)
        {
            if (category == null)
            {
                return BadRequest("Dane kategorii są nieprawidłowe.");
            }

            var addedCategory = await _kategoriaRepository.AddAsync(category);
            return CreatedAtAction(nameof(GetById), new { id = addedCategory.IdKategorii }, addedCategory);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Category category)
        {
            if (category == null || id != category.IdKategorii)
            {
                return BadRequest("Dane kategorii są nieprawidłowe.");
            }

            var updatedCategory = await _kategoriaRepository.UpdateAsync(category);
            if (updatedCategory == null)
            {
                return NotFound($"Kategoria o ID {id} nie została znaleziona.");
            }
            return Ok(updatedCategory);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _kategoriaRepository.DeleteAsync(id);
            if (!result)
            {
                return NotFound($"Kategoria o ID {id} nie została znaleziona.");
            }
            return NoContent();
        }
    }
}
