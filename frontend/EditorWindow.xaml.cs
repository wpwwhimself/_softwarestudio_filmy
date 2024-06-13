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
      Id = id;

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
        MessageBox.Show("Rok musi znajdować się w przedziale 1900 do 2200", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        return;
      }

      if (FilmTitle == string.Empty)
      {
        MessageBox.Show("Tytuł nie może być pusty", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        return;
      }

      if (FilmTitle.Length > 200)
      {
        MessageBox.Show("Tytuł musi być krótszy niż 200 znaków", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        return;
      }

      try
      {
        Film film = new Film
        {
          Title = FilmTitle,
          Director = Director,
          Year = Year,
          Rate = Rate
        };

        var res = (Id == null)
          ? await httpClient.PostAsync(
            "http://localhost:5043/api/Films",
            new StringContent(
              JsonSerializer.Serialize(film),
              Encoding.UTF8,
              "application/json"
            )
          )
          : await httpClient.PutAsync(
            $"http://localhost:5043/api/Films/{Id}",
            new StringContent(
              JsonSerializer.Serialize(film),
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
        MessageBox.Show($"Błąd: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
      DialogResult = false;
      Close();
    }
  }
}
