﻿using System.Net.Http;
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
        MessageBox.Show("Add button clicked!");
    }

    private void RefreshButton_Click(object sender, RoutedEventArgs e)
    {
        LoadVideos();
    }

    private void DownloadButton_Click(object sender, RoutedEventArgs e)
    {

    }

    private void EditButton_Click(object sender, RoutedEventArgs e)
    {
        var video = (Video)VideoDataGrid.SelectedItem;
        if (video != null)
        {
            MessageBox.Show($"Edit button clicked for {video.Title}");
        }
    }

    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        var video = (Video)VideoDataGrid.SelectedItem;
        if (video != null)
        {
            MessageBox.Show($"Delete button clicked for {video.Title}");
        }
    }
}