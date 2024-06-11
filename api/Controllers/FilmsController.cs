using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.Data;
using SharedModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class FilmsController : ControllerBase
  {
    private readonly AppDbContext _context;

    public FilmsController(AppDbContext context)
    {
      _context = context;
    }

    // GET: api/Films
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Film>>> GetFilms()
    {
      return await _context.Films.ToListAsync();
    }

    // GET: api/Films/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Film>> GetFilm(int id)
    {
      var film = await _context.Films.FindAsync(id);

      if (film == null)
      {
        return NotFound();
      }

      return film;
    }

    // POST: api/Films
    [HttpPost]
    public async Task<ActionResult<Film>> PostFilm(Film film)
    {
      _context.Films.Add(film);
      await _context.SaveChangesAsync();

      return CreatedAtAction(nameof(GetFilm), new { id = film.Id }, film);
    }

    // PUT: api/Films/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutFilm(int id, Film film)
    {
      Film existingFilm = await _context.Films.FindAsync(id);
      if (existingFilm == null)
      {
        return NotFound();
      }

      existingFilm.Title = film.Title;
      existingFilm.Director = film.Director;
      existingFilm.Year = film.Year;
      existingFilm.Rate = film.Rate;

      await _context.SaveChangesAsync();

      return Ok(existingFilm);
    }

    // DELETE: api/Films/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFilm(int id)
    {
      var film = await _context.Films.FindAsync(id);
      if (film == null)
      {
        return NotFound();
      }

      _context.Films.Remove(film);
      await _context.SaveChangesAsync();

      return Ok();
    }
  }
}
