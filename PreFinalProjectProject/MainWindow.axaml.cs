using System;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace PreFinalProjectProject;
public partial class MainWindow : Window
{
    private LocationData locationData = new LocationData();
    public MainWindow()
    {
        InitializeComponent();
    }

    private void GetTheWeather(object? sender, RoutedEventArgs e)
    {
        Console.WriteLine(locationData.GetIpAddress());
    }
}