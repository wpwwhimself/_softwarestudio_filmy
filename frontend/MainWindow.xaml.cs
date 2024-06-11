using System.Net.Http;
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
            var videos = await _httpClient.GetFromJsonAsync<List<Video>>("http://localhost:5000/api/videos");
            VideoDataGrid.ItemsSource = videos;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}");
        }
    }

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Add button clicked!");
    }
}