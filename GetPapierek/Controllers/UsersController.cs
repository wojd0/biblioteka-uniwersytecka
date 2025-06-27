using GetPapierek.Models;
using GetPapierek.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GetPapierek.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userRepository.GetAllAsync();
            foreach (var user in users)
            {
                user.Password = null;
            }
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound($"Użytkownik o ID {id} nie został znaleziony.");
            }

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

            var existingUser = await _userRepository.GetByEmailAsync(user.Email);
            if (existingUser != null)
            {
                return BadRequest("Użytkownik z tym adresem email już istnieje.");
            }

            var addedUser = await _userRepository.AddAsync(user);

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

            var updatedUser = await _userRepository.UpdateAsync(user);
            if (updatedUser == null)
            {
                return NotFound($"Użytkownik o ID {id} nie został znaleziony.");
            }

            updatedUser.Password = null;
            return Ok(updatedUser);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _userRepository.DeleteAsync(id);
            if (!result)
            {
                return NotFound($"Użytkownik o ID {id} nie został znaleziony.");
            }
            return NoContent();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userRepository.AuthenticateAsync(model.Email, model.Haslo);
            if (user == null)
            {
                return Unauthorized("Nieprawidłowy email lub hasło.");
            }

            user.Password = null;
            return Ok(new { message = "Zalogowano pomyślnie", user });
        }
    }

    public class LoginModel
    {
        public required string Email { get; set; }
        public required string Haslo { get; set; }
    }
}
