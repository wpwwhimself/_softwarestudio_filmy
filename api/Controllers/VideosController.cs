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
      Video existingVideo = await _context.Videos.FindAsync(id);
      if (existingVideo == null)
      {
        return NotFound();
      }

      existingVideo.Title = video.Title;
      existingVideo.Director = video.Director;
      existingVideo.Year = video.Year;
      existingVideo.Rate = video.Rate;

      await _context.SaveChangesAsync();

      return Ok(existingVideo);
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

      return Ok();
    }
  }
}
