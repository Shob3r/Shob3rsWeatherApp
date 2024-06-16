using System;
using System.Collections.Generic;
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
        await futureForecast.setWeatherData();
        await currentWeather.setWeatherData();

        greeting.Text = $"Good {getTime()}, {Environment.UserName}";
        usersLocation.Text = $"{LocationInformation.currentCity}, {LocationInformation.fullCountryName}";
        coordinates.Text = $"({LocationInformation.latitude}, {LocationInformation.longitude})";
        weatherRightNow.Text = $"{currentWeather.tempNow}\u00b0{currentWeather.tempUnit}";
        weatherImage.Source = new Bitmap(AssetLoader.Open(new Uri($"avares://Shob3rsWeatherApp/Assets/Images/{getWeatherImageName(currentWeather.weatherDescription)}.png")));
        if (currentWeather.detailedWeatherDescription != null) weatherDescription.Text = textInfo.ToTitleCase(currentWeather.detailedWeatherDescription);

        List<TextBlock> futureWeatherDate = [futureWeatherCol0Date, futureWeatherCol1Date, futureWeatherCol2Date, futureWeatherCol3Date];
        List<TextBlock> futureWeatherTemp = [futureWeatherCol0Temp, futureWeatherCol1Temp, futureWeatherCol2Temp, futureWeatherCol3Temp];
        List<Image> futureWeatherImage = [futureWeatherCol0Image, futureWeatherCol1Image, futureWeatherCol2Image, futureWeatherCol3Image];

        for (int i = 0; i < 4; i++)
        {
            futureWeatherDate[i].Text = i == 0 ? "Tomorrow" : getDayOfWeekInFuture(i + 1);
            futureWeatherImage[i].Source = new Bitmap(AssetLoader.Open(new Uri($"avares://Shob3rsWeatherApp/Assets/Images/{getWeatherImageName(futureForecast.futureWeatherDescriptions!.ElementAt(i))}.png")));
            futureWeatherTemp[i].Text = $"{futureForecast.futureTemperatures!.ElementAt(i)}\u00b0{currentWeather.tempUnit}";
        }
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

    private void refreshWeatherData(object? sender, RoutedEventArgs e)
    {
        setContentTask = setMenuContent();
    }
}