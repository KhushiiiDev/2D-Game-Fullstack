using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GameBackendAPI.Data;
using GameBackendAPI.Models;

namespace GameBackendAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CharactersController : ControllerBase
    {
        private readonly GameDbContext _context;
        public CharactersController(GameDbContext context) { _context = context; }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _context.Characters.Include(c => c.User).ToListAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var character = await _context.Characters.Include(c => c.User).FirstOrDefaultAsync(c => c.Id == id);
            return character == null ? NotFound() : Ok(character);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Character character)
        {
            _context.Characters.Add(character);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = character.Id }, character);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Character character)
        {
            if (id != character.Id) return BadRequest();
            _context.Entry(character).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var character = await _context.Characters.FindAsync(id);
            if (character == null) return NotFound();
            _context.Characters.Remove(character);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
