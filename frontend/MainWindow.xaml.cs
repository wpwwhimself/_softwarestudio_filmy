using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
            var films = await _httpClient.GetFromJsonAsync<List<Film>>("http://localhost:5043/api/films");
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
            await _httpClient.PostAsync("http://localhost:5043/api/PullFromAPI/", null);
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
                $"Delete film {film.Title}?",
                "Confirm",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (res == MessageBoxResult.Yes)
            {
                try
                {
                    await _httpClient.DeleteAsync($"http://localhost:5043/api/Films/{film.Id}");
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