using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.Data;
using SharedModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class PullFromAPIController : ControllerBase
  {
    private readonly AppDbContext _context;

    public PullFromAPIController(AppDbContext context)
    {
      _context = context;
    }

    [HttpPost]
    public async Task<ActionResult> PullFromAPI()
    {
      HttpClient _httpClient = new HttpClient();

      try
      {
        var videos = await _httpClient.GetFromJsonAsync<List<MyMoviesMovie>>("https://filmy.programdemo.pl/MyMovies");

        foreach (var video in videos)
        {
          Video? existingVideo = _context.Videos.Where(ev => ev.ImportId == video.id).FirstOrDefault();
          if (existingVideo == null)
          {
            _context.Videos.Add(new Video
            {
              Title = video.title,
              Director = video.director,
              Year = video.year,
              Rate = video.rate,
              ImportId = video.id
            });
          }
        }
        
        await _context.SaveChangesAsync();
        return Created();
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }
  }
}
