using GetPapierek.Models;
using GetPapierek.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GetPapierek.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _uzytkownikRepository;

        public UsersController(IUserRepository uzytkownikRepository)
        {
            _uzytkownikRepository = uzytkownikRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _uzytkownikRepository.GetAllAsync();
            // For security, don't return passwords in the API response
            foreach (var user in users)
            {
                user.Password = null;
            }
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _uzytkownikRepository.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound($"Użytkownik o ID {id} nie został znaleziony.");
            }

            // For security, don't return password in the API response
            user.Password = null;
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest("Dane użytkownika są nieprawidłowe.");
            }

            // Check if email already exists
            var existingUser = await _uzytkownikRepository.GetByEmailAsync(user.Email);
            if (existingUser != null)
            {
                return BadRequest("Użytkownik z tym adresem email już istnieje.");
            }

            var addedUser = await _uzytkownikRepository.AddAsync(user);

            // For security, don't return password in the API response
            addedUser.Password = null;
            return CreatedAtAction(nameof(GetById), new { id = addedUser.UserId }, addedUser);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] User user)
        {
            if (user == null || id != user.UserId)
            {
                return BadRequest("Dane użytkownika są nieprawidłowe.");
            }

            var updatedUser = await _uzytkownikRepository.UpdateAsync(user);
            if (updatedUser == null)
            {
                return NotFound($"Użytkownik o ID {id} nie został znaleziony.");
            }

            // For security, don't return password in the API response
            updatedUser.Password = null;
            return Ok(updatedUser);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _uzytkownikRepository.DeleteAsync(id);
            if (!result)
            {
                return NotFound($"Użytkownik o ID {id} nie został znaleziony.");
            }
            return NoContent();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _uzytkownikRepository.AuthenticateAsync(model.Email, model.Haslo);
            if (user == null)
            {
                return Unauthorized("Nieprawidłowy email lub hasło.");
            }

            // In a real application, we would generate a JWT token here
            // but for simplicity we'll just return the user without the password
            user.Password = null;
            return Ok(new { message = "Zalogowano pomyślnie", user });
        }
    }

    public class LoginModel
    {
        public string Email { get; set; }
        public string Haslo { get; set; }
    }
}
