namespace api.Models
{
  public class MyMoviesMovie
  {
    public int id { get; set; }
    public string title { get; set; }
    public string? director { get; set; }
    public int? year { get; set; }
    public double? rate { get; set; }
  }
}