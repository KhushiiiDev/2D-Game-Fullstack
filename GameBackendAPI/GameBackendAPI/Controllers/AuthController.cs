using Microsoft.AspNetCore.Mvc;
using GameBackendAPI.Models;
using GameBackendAPI.Data;
using GameBackendAPI.Authentication;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace GameBackendAPI.Controllers
{

    public class LoginDto
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }

    public class RegisterDto
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly GameDbContext _context;
        private readonly JwtService _jwtService;

        public AuthController(GameDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest("All fields are required.");

            if (await _context.Users.AnyAsync(u => u.Username == dto.Username || u.Email == dto.Email))
                return BadRequest("Username or Email already exists.");

            var user = new User
            {
                Username = dto.Username!,
                Email = dto.Email!,
                PasswordHash = HashPassword(dto.Password!),
                Role = "User"
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Registration successful." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == login.Username);
            if (user == null || string.IsNullOrEmpty(login.Password) || !VerifyPassword(login.Password, user.PasswordHash))
                return Unauthorized("Invalid credentials.");

            var token = _jwtService.GenerateToken(user);
            return Ok(new { token });
        }

        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        private bool VerifyPassword(string input, string hash)
        {
            return HashPassword(input) == hash;
        }
    }
}
