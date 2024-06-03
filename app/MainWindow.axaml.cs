using System;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Shob3rsWeatherApp;
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        setMainPageContent();
    }
    
    private void setMainPageContent()
    {
        greeting.Text = $"Good {getTime()} {Environment.UserName}";
    }
    
    private string getTime()
    {
        var currentTime = DateTime.Now;
        int currentHour = currentTime.Hour;
        
        Console.WriteLine(currentHour);

        return currentHour switch
        {
            >= 6 and < 12 => "Morning",
            >= 12 and < 19 => "Afternoon",
            _ => "Evening"
        };
    }

    private void CloseMenu(object? sender, RoutedEventArgs e)
    {
        sideMenu.IsPaneOpen = !sideMenu.IsPaneOpen;
    }
}
