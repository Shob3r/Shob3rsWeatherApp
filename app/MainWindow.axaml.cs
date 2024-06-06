using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Shob3rsWeatherApp;
public partial class MainWindow : Window
{
    OpenWeatherMapData openWeatherMapData;
    private Task setContentTask;
    public MainWindow()
    {
        openWeatherMapData = new OpenWeatherMapData();
        InitializeComponent();
        setContentTask = setMenuContent();
    }

    private async Task setMenuContent()
    {
        await openWeatherMapData.setWeatherData();
        greeting.Text = $"Good {getTime()}, {Environment.UserName}";
        weatherRightNow.Text = $"{openWeatherMapData.tempNow}\u00b0";
    }
    
    
    
    private string getTime()
    {
        var currentTime = DateTime.Now;
        int currentHour = currentTime.Hour;

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

    private void refreshWeatherData(object? sender, RoutedEventArgs e)
    {
        Console.WriteLine("Refreshing Weather Data......");
        Task.Run(setMenuContent);
    }
}
