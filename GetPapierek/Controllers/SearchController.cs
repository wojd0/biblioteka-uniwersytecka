using GetPapierek.Models;
using GetPapierek.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GetPapierek.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly ICategoryRepository _categoryRepository;

        public SearchController(
            IBookRepository bookRepository,
            ICategoryRepository categoryRepository)
        {
            _bookRepository = bookRepository;
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Search query cannot be empty.");
            }

            var books = await _bookRepository.SearchAsync(query);

            var results = books
                .GroupBy(b => b.Category?.Name ?? "Uncategorized")
                .Select(g => new
                {
                    Category = g.Key,
                    Books = g.Select(b => new
                    {
                        b.BookId,
                        b.Title,
                        b.Author,
                        b.PublicationYear,
                        b.Shelf
                    }).ToList()
                })
                .ToList();

            return Ok(new
            {
                Query = query,
                TotalCount = books.Count,
                GroupedResults = results
            });
        }

        [HttpGet("advanced")]
        public async Task<IActionResult> AdvancedSearch(
            [FromQuery] string? title = null,
            [FromQuery] string? author = null,
            [FromQuery] int? categoryId = null,
            [FromQuery] int? yearFrom = null,
            [FromQuery] int? yearTo = null)
        {
            var allBooks = await _bookRepository.GetAllAsync();

            var filteredBooks = allBooks.AsQueryable();

            if (!string.IsNullOrWhiteSpace(title))
            {
                title = title.ToLower();
                filteredBooks = filteredBooks.Where(b => b.Title.ToLower().Contains(title));
            }

            if (!string.IsNullOrWhiteSpace(author))
            {
                author = author.ToLower();
                filteredBooks = filteredBooks.Where(b => b.Author.ToLower().Contains(author));
            }

            if (categoryId.HasValue)
            {
                filteredBooks = filteredBooks.Where(b => b.CategoryId == categoryId.Value);
            }

            if (yearFrom.HasValue)
            {
                filteredBooks = filteredBooks.Where(b => b.PublicationYear >= yearFrom.Value);
            }

            if (yearTo.HasValue)
            {
                filteredBooks = filteredBooks.Where(b => b.PublicationYear <= yearTo.Value);
            }

            var results = filteredBooks.ToList();

            return Ok(new
            {
                TotalCount = results.Count,
                Books = results
            });
        }
    }
}
