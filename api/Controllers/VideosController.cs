using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.Data;
using api.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class VideosController : ControllerBase
  {
    private readonly AppDbContext _context;

    public VideosController(AppDbContext context)
    {
      _context = context;
    }

    // GET: api/Videos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Video>>> GetVideos()
    {
      return await _context.Videos.ToListAsync();
    }

    // GET: api/Videos/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Video>> GetVideo(int id)
    {
      var video = await _context.Videos.FindAsync(id);

      if (video == null)
      {
          return NotFound();
      }

      return video;
    }

    // POST: api/Videos
    [HttpPost]
    public async Task<ActionResult<Video>> PostVideo(Video video)
    {
      _context.Videos.Add(video);
      await _context.SaveChangesAsync();

      return CreatedAtAction(nameof(GetVideo), new { id = video.Id }, video);
    }

    // PUT: api/Videos/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutVideo(int id, Video video)
    {
      if (id != video.Id)
      {
        return BadRequest();
      }

      _context.Entry(video).State = EntityState.Modified;

      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!VideoExists(id))
        {
          return NotFound();
        }
        else
        {
          throw;
        }
      }

      return NoContent();
    }

    // DELETE: api/Videos/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteVideo(int id)
    {
      var video = await _context.Videos.FindAsync(id);
      if (video == null)
      {
        return NotFound();
      }

      _context.Videos.Remove(video);
      await _context.SaveChangesAsync();

      return NoContent();
    }

    private bool VideoExists(int id)
    {
      return _context.Videos.Any(e => e.Id == id);
    }
  }
}
