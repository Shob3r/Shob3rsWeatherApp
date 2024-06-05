using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Shob3rsWeatherApp;
public partial class MainWindow : Window
{
    OpenWeatherMapData openWeatherMapData;
    public MainWindow()
    {
        InitializeComponent();
        setMainPageContent();
        openWeatherMapData = new OpenWeatherMapData();
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

    private void refreshWeatherData(object? sender, RoutedEventArgs e)
    {
        Console.WriteLine("Refreshing Weather Data......");
        Task.Run(() => openWeatherMapData.setWeatherData());
    }
}
