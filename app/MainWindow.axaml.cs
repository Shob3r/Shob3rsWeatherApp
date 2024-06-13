using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace Shob3rsWeatherApp;

public partial class MainWindow : Window
{
    private readonly OpenWeatherData currentWeather;
    private readonly OpenWeatherFutureForecasting futureForecast;
    private Task setContentTask;

    public MainWindow()
    {
        currentWeather = new OpenWeatherData();
        futureForecast = new OpenWeatherFutureForecasting();

        InitializeComponent();
        setContentTask = setMenuContent();
    }

    private async Task setMenuContent()
    {
        TextInfo textInfo = new CultureInfo("en-CA", false).TextInfo;
        await LocationInformation.setLocationData();
        await currentWeather.setWeatherData();

        greeting.Text = $"Good {getTime()}, {Environment.UserName}";
        usersLocation.Text = $"{LocationInformation.currentCity}, {LocationInformation.fullCountryName}";
        weatherRightNow.Text = $"{currentWeather.tempNow}\u00b0{currentWeather.tempUnit}";
        weatherImage.Source = new Bitmap(AssetLoader.Open(new Uri($"avares://Shob3rsWeatherApp/Assets/Images/{getWeatherImageName(currentWeather.weatherDescription)}.png")));
        if (currentWeather.detailedWeatherDescription != null) weatherDescription.Text = textInfo.ToTitleCase(currentWeather.detailedWeatherDescription);

        await setFutureForecastContent();
    }

    private async Task setFutureForecastContent()
    {
        await futureForecast.setWeatherData();
        
        futureWeatherCol0Date.Text = "Tomorrow";
        futureWeatherCol0Image.Source = new Bitmap(AssetLoader.Open(new Uri($"avares://Shob3rsWeatherApp/Assets/Images/{getWeatherImageName(futureForecast.futureWeatherDescriptions!.ElementAt(0))}.png")));
        futureWeatherCol0Temp.Text = $"{futureForecast.futureTemperatures!.ElementAt(0)}\u00b0{currentWeather.tempUnit}";
        
        futureWeatherCol1Date.Text = getDayOfWeekInFuture(2);
        futureWeatherCol1Image.Source = new Bitmap(AssetLoader.Open(new Uri($"avares://Shob3rsWeatherApp/Assets/Images/{getWeatherImageName(futureForecast.futureWeatherDescriptions!.ElementAt(1))}.png")));
        futureWeatherCol1Temp.Text = $"{futureForecast.futureTemperatures!.ElementAt(1)}\u00b0{currentWeather.tempUnit}";
        
        futureWeatherCol2Date.Text = getDayOfWeekInFuture(3);
        futureWeatherCol2Image.Source = new Bitmap(AssetLoader.Open(new Uri($"avares://Shob3rsWeatherApp/Assets/Images/{getWeatherImageName(futureForecast.futureWeatherDescriptions!.ElementAt(2))}.png")));
        futureWeatherCol2Temp.Text = $"{futureForecast.futureTemperatures!.ElementAt(2)}\u00b0{currentWeather.tempUnit}";
        
        futureWeatherCol3Date.Text = getDayOfWeekInFuture(4);
        futureWeatherCol3Image.Source = new Bitmap(AssetLoader.Open(new Uri($"avares://Shob3rsWeatherApp/Assets/Images/{getWeatherImageName(futureForecast.futureWeatherDescriptions!.ElementAt(3))}.png")));
        futureWeatherCol3Temp.Text = $"{futureForecast.futureTemperatures!.ElementAt(3)}\u00b0{currentWeather.tempUnit}";
    }
    
    private string getWeatherImageName(string? inputWeatherDescription)
    {
        return inputWeatherDescription!.ToLower() switch
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

    private string getDayOfWeekInFuture(int days)
    {
        DateTime thePresent = DateTime.Today;
        DateTime futureDate = thePresent.AddDays(days);

        return futureDate.DayOfWeek.ToString();
    }
    
    private string getTime(bool onlyDayAndNight = false)
    {
        DateTime currentTime = DateTime.Now;
        int currentHour = currentTime.Hour;

        if (onlyDayAndNight)
            return currentHour switch
            {
                >= 6 and < 18 => "day",
                _ => "night"
            };

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
        getActiveView().IsVisible = false;
        weatherSearchContent.IsVisible = true;
    }

    private void OpenWeatherHere(object? sender, RoutedEventArgs e)
    {
        getActiveView().IsVisible = false;
        weatherHereContent.IsVisible = true;
    }

    private void OpenSettings(object? sender, RoutedEventArgs e)
    {
        getActiveView().IsVisible = false;
        settingsContent.IsVisible = true;
    }

    private Grid getActiveView()
    {
        Grid[] appMenuViews = { weatherHereContent, weatherSearchContent, settingsContent };
        foreach (Grid t in appMenuViews)
            if (t.IsVisible)
                return t;

        Console.WriteLine("Errrmm... Why aren't any of the views active....");
        throw new Exception();
    }
}