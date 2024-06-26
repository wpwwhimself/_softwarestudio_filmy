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
        var films = await _httpClient.GetFromJsonAsync<List<MyMoviesMovie>>("https://filmy.programdemo.pl/MyMovies");

        foreach (var film in films)
        {
          Film? existingFilm = _context.Films.Where(ev => ev.ImportId == film.id).FirstOrDefault();
          if (existingFilm == null)
          {
            _context.Films.Add(new Film
            {
              Title = film.title,
              Director = film.director,
              Year = film.year,
              Rate = film.rate,
              ImportId = film.id
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
