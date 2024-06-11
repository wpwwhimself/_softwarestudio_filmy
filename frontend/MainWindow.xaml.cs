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
        LoadVideos();
    }

    private async void LoadVideos()
    {
        try
        {
            var videos = await _httpClient.GetFromJsonAsync<List<Video>>("http://localhost:5043/api/videos");
            VideoDataGrid.ItemsSource = videos;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}");
        }
    }

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        EditorWindow editorWindow = new EditorWindow();
        if (editorWindow.ShowDialog() == true)
        {
            LoadVideos();
        }
    }

    private void RefreshButton_Click(object sender, RoutedEventArgs e)
    {
        LoadVideos();
    }

    private async void DownloadButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            await _httpClient.PostAsync("http://localhost:5043/api/PullFromAPI/", null);
            LoadVideos();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}");
        }
    }

    private void EditButton_Click(object sender, RoutedEventArgs e)
    {
        var video = (sender as Button).DataContext as Video;
        if (video == null) return;

        EditorWindow editorWindow = new EditorWindow(
            video.Id,
            video.Title,
            video.Director,
            video.Year,
            video.Rate
        );

        if (editorWindow.ShowDialog() == true)
        {
            LoadVideos();
        }
    }

    private async void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        var video = (sender as Button).DataContext as Video;
        if (video != null)
        {
            MessageBoxResult res = MessageBox.Show(
                $"Delete film {video.Title}?",
                "Confirm",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (res == MessageBoxResult.Yes)
            {
                try
                {
                    await _httpClient.DeleteAsync($"http://localhost:5043/api/Videos/{video.Id}");
                    LoadVideos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }
    }
}