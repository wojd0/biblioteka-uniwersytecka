using GetPapierek.Models;
using GetPapierek.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GetPapierek.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound($"Category with ID {id} was not found.");
            }
            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Category category)
        {
            if (category == null)
            {
                return BadRequest("Category data is invalid.");
            }

            var addedCategory = await _categoryRepository.AddAsync(category);
            return CreatedAtAction(nameof(GetById), new { id = addedCategory.Id }, addedCategory);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Category category)
        {
            if (category == null || id != category.Id)
            {
                return BadRequest("Category data is invalid.");
            }

            var updatedCategory = await _categoryRepository.UpdateAsync(category);
            if (updatedCategory == null)
            {
                return NotFound($"Category with ID {id} was not found.");
            }
            return Ok(updatedCategory);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _categoryRepository.DeleteAsync(id);
            if (!result)
            {
                return NotFound($"Category with ID {id} was not found.");
            }
            return NoContent();
        }
    }
}
