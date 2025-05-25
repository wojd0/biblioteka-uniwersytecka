using GetPapierek.Models;
using GetPapierek.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GetPapierek.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly IBookRepository _ksiazkaRepository;
        private readonly ICategoryRepository _kategoriaRepository;

        public SearchController(
            IBookRepository ksiazkaRepository,
            ICategoryRepository kategoriaRepository)
        {
            _ksiazkaRepository = ksiazkaRepository;
            _kategoriaRepository = kategoriaRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Zapytanie wyszukiwania nie może być puste.");
            }

            var books = await _ksiazkaRepository.SearchAsync(query);

            // Group results by category for better organization
            var results = books
                .GroupBy(b => b.Category?.NazwaKategorii ?? "Bez kategorii")
                .Select(g => new
                {
                    Kategoria = g.Key,
                    Ksiazki = g.Select(b => new
                    {
                        b.IdKsiazki,
                        b.Tytul,
                        b.Autor,
                        b.RokWydania,
                        b.Pulka
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
            [FromQuery] string title = null,
            [FromQuery] string author = null,
            [FromQuery] int? categoryId = null,
            [FromQuery] int? yearFrom = null,
            [FromQuery] int? yearTo = null)
        {
            // Get all books first
            var allBooks = await _ksiazkaRepository.GetAllAsync();

            // Apply filters
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
