using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows;
using SharedModels;

namespace frontend
{
  public partial class EditorWindow : Window
  {
    private readonly HttpClient httpClient = new HttpClient();

    public int? Id { get; set; }
    public string FilmTitle => TitleTextBox.Text;
    public string Director => DirectorTextBox.Text;
    public int? Year => int.TryParse(YearTextBox.Text, out int year) ? year : (int?)null;
    public double? Rate => double.TryParse(RateTextBox.Text, out double rate) ? rate : (double?)null;

    // adding new film
    public EditorWindow()
    {
      InitializeComponent();
      Title = "Add new film";
    }

    // editing existing film
    public EditorWindow(int id, string title, string? director, int? year, double? rate)
    {
      InitializeComponent();
      Title = $"Edit film: {title}";
      this.Id = id;

      TitleTextBox.Text = title;
      DirectorTextBox.Text = director;
      YearTextBox.Text = year?.ToString();
      RateTextBox.Text = rate?.ToString();
    }

    private async void SaveButton_Click(object sender, RoutedEventArgs e)
    {
      // validate inputs
      if (Year < 1900 || Year > 2200)
      {
        MessageBox.Show("Year must be between 1900 and 2200", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        return;
      }

      if (FilmTitle == string.Empty)
      {
        MessageBox.Show("Title cannot be empty", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        return;
      }

      if (FilmTitle.Length > 200)
      {
        MessageBox.Show("Title cannot be longer than 200 characters", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        return;
      }

      try
      {
        Video video = new Video
        {
          Title = FilmTitle,
          Director = Director,
          Year = Year,
          Rate = Rate
        };

        var res = (Id == null)
          ? await httpClient.PostAsync(
            "http://localhost:5043/api/Videos",
            new StringContent(
              JsonSerializer.Serialize(video),
              Encoding.UTF8,
              "application/json"
            )
          )
          : await httpClient.PutAsync(
            $"http://localhost:5043/api/Videos/{Id}",
            new StringContent(
              JsonSerializer.Serialize(video),
              Encoding.UTF8,
              "application/json"
            )
          );

        if (res.IsSuccessStatusCode)
        {
          DialogResult = true;
          Close();
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
      DialogResult = false;
      Close();
    }
  }
}