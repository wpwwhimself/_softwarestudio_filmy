using Microsoft.EntityFrameworkCore;
using api.Models;

namespace api.Data
{
  public class AppDbContext : DbContext
  {
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Video> Videos { get; set; }
  }
}
