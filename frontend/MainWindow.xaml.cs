using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using System.Windows.Controls;
using SharedModels;

namespace frontend;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly HttpClient _httpClient;

    public MainWindow()
    {
        InitializeComponent();
        _httpClient = new HttpClient();
        LoadFilms();
    }

    private async void LoadFilms()
    {
        try
        {
            var films = await _httpClient.GetFromJsonAsync<List<Film>>(App.ApiUrl + "films");
            FilmDataGrid.ItemsSource = films;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Błąd: {ex.Message}");
        }
    }

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        EditorWindow editorWindow = new EditorWindow();
        if (editorWindow.ShowDialog() == true)
        {
            LoadFilms();
        }
    }

    private async void DownloadButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            await _httpClient.PostAsync(App.ApiUrl + "PullFromAPI/", null);
            LoadFilms();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Błąd: {ex.Message}");
        }
    }

    private void EditButton_Click(object sender, RoutedEventArgs e)
    {
        var film = (sender as Button).DataContext as Film;
        if (film == null) return;

        EditorWindow editorWindow = new EditorWindow(
            film.Id,
            film.Title,
            film.Director,
            film.Year,
            film.Rate
        );

        if (editorWindow.ShowDialog() == true)
        {
            LoadFilms();
        }
    }

    private async void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        var film = (sender as Button).DataContext as Film;
        if (film != null)
        {
            MessageBoxResult res = MessageBox.Show(
                $"Usunąć film {film.Title}?",
                "Potwierdź operację",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (res == MessageBoxResult.Yes)
            {
                try
                {
                    await _httpClient.DeleteAsync(App.ApiUrl + $"Films/{film.Id}");
                    LoadFilms();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Błąd: {ex.Message}");
                }
            }
        }
    }
}