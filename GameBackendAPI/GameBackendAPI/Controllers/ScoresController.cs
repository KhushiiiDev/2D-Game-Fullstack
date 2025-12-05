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
    public class ScoresController : ControllerBase
    {
        private readonly GameDbContext _context;
        public ScoresController(GameDbContext context) { _context = context; }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _context.Scores.Include(s => s.User).ToListAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var score = await _context.Scores.Include(s => s.User).FirstOrDefaultAsync(s => s.Id == id);
            return score == null ? NotFound() : Ok(score);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Score score)
        {
            _context.Scores.Add(score);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = score.Id }, score);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Score score)
        {
            if (id != score.Id) return BadRequest();
            _context.Entry(score).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var score = await _context.Scores.FindAsync(id);
            if (score == null) return NotFound();
            _context.Scores.Remove(score);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
