namespace SharedModels
{
  public class Video
  {
    public int Id { get; set; }
    public int? ImportId { get; set; }
    public string Title { get; set; }
    public string? Director { get; set; }
    public int? Year { get; set; }
    public double? Rate { get; set; }
  }
}
