using System;
using System.Globalization;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace Shob3rsWeatherApp;
public partial class MainWindow : Window
{
    private readonly OpenWeatherMapData openWeatherMapData;
    private Task setContentTask;
    public MainWindow()
    {
        openWeatherMapData = new OpenWeatherMapData();
        InitializeComponent();
        setContentTask = setMenuContent();
    }

    private async Task setMenuContent()
    {
        TextInfo textInfo = new CultureInfo("en-CA", false).TextInfo;
        await openWeatherMapData.setWeatherData();
        greeting.Text = $"Good {getTime()}, {Environment.UserName}";
        weatherRightNow.Text = $"{openWeatherMapData.tempNow}\u00b0{openWeatherMapData.tempUnit}";
        weatherImage.Source = new Bitmap(AssetLoader.Open(new Uri($"avares://Shob3rsWeatherApp/Assets/Images/{getWeatherImageName()}.png")));
        if (openWeatherMapData.detailedWeatherDescription != null) weatherDescription.Text = textInfo.ToTitleCase(openWeatherMapData.detailedWeatherDescription);
    }

    private string getWeatherImageName()
    {
        string? currentWeather = openWeatherMapData.weatherDescription;
        return currentWeather!.ToLower() switch
        {
            "clear" => $"clear-{getTime(true)}",
            "drizzle" => "rainy",
            "rain" => "storm",
            "snow" => "snowy",
            "mist" => "overcast",
            "haze" => "overcast",
            "fog" => "fog",
            "ash" => "fog",
            "squall" => "windy",
            "tornado" => "tornado",
            "clouds" => $"cloudy-{getTime(true)}",
            _ => "clear-day"
        };
    }
    
    private string getTime(bool onlyDayAndNight = false)
    {
        DateTime currentTime = DateTime.Now;
        int currentHour = currentTime.Hour;

        if (onlyDayAndNight)
        {
            return currentHour switch
            {
                >= 6 and < 18 => "day",
                _ => "night"
            };
        }
        
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
        setContentTask = setMenuContent();
    }

    private void OpenWeatherSearch(object? sender, RoutedEventArgs e)
    {
        sideMenu.IsVisible = false;
    }
}
