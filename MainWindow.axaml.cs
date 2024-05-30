using System;
using System.IO;
using Avalonia.Controls;
using Avalonia.Interactivity;


namespace Shob3rsWeatherApp;
public partial class MainWindow : Window
{
    private LocationData locationData = new LocationData();
    private JsonParser openWeatherMapKey = new JsonParser(File.ReadAllText("../../../config.json"));
    private bool isAmerican;
    
    public MainWindow()
    {
        InitializeComponent();
        setMainPageContent();
    }
    
    private void openMainMenu(object? sender, RoutedEventArgs e)
    {
        
    }

    private void openWeatherSearch(object? sender, RoutedEventArgs e)
    {
        
    }

    private void openSettingsMenu(object? sender, RoutedEventArgs e)
    {
        
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
    
    private int toCelsius(float kelvin)
    {  
        return (int)kelvin - 273;
    }  

    private int toFahrenheit(float kelvin)
    {
        // Convert to C to make it easier
        int celsius = toCelsius(kelvin);
        return (int)Math.Round(celsius * ((float) 9 / 5) + 32);
    }

    private void CloseMenu(object? sender, RoutedEventArgs e)
    {
        sideMenu.IsPaneOpen = !sideMenu.IsPaneOpen;
    }
}
