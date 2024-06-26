using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Avalonia.Platform;


namespace Shob3rsWeatherApp;
// ReSharper disable NotAccessedField.Local
public partial class MainWindow : Window
{
    private MainWindowUtils mainWindowUtils;
    private readonly OpenWeatherData currentWeather = new OpenWeatherData();
    private readonly TextInfo textInfo = new CultureInfo("en-CA", false).TextInfo;

    private Task setContentTask;
    
    #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public MainWindow()
    {
        InitializeComponent();
        setContentTask = setMenuContent();
    }

    private async Task setMenuContent()
    {
        // Create a text formatter, so I can use ToTileCase() when I need to, then wait for the classes to set their data, so this method can execute properly
        await LocationInformation.setLocationData();
        await currentWeather.updateWeatherData();
        mainWindowUtils = new MainWindowUtils(currentWeather);

        setGreetingContent();
        setCurrentWeatherContent();
        setForecastContent();
        setWidgetContent();
    }

    private void setGreetingContent()
    {
        // The Greeting and location stuff at the top of the window
        greeting.Text = $"Good {mainWindowUtils.getTime()}, {Environment.UserName}";
        usersLocation.Text = $"{LocationInformation.currentCity}, {LocationInformation.fullCountryName}";
        coordinates.Text = $"({LocationInformation.latitude}, {LocationInformation.longitude})";
    }
    
    private void setCurrentWeatherContent()
    {
        weatherRightNow.Text = $"{currentWeather.tempNow}\u00b0{currentWeather.tempUnit}";
        weatherImage.Source = new Bitmap(AssetLoader.Open(new Uri($"avares://Shob3rsWeatherApp/Assets/Images/{mainWindowUtils.getWeatherImageName(currentWeather.weatherDescription)}.png")));
        weatherDescription.Text = textInfo.ToTitleCase(currentWeather.detailedWeatherDescription);
        
        todaysWeather.Text = currentWeather.weatherOutlook;
    }

    private void setForecastContent()
    {
        List<TextBlock> futureWeatherDate = [futureWeatherCol0Date, futureWeatherCol1Date, futureWeatherCol2Date, futureWeatherCol3Date];
        List<TextBlock> futureMaxTemp = [weatherForecastCol0MaxTemp, weatherForecastCol1MaxTemp, weatherForecastCol2MaxTemp, weatherForecastCol3MaxTemp];
        List <TextBlock> futureMinTemp = [weatherForecastCol0MinTemp, weatherForecastCol1MinTemp, weatherForecastCol2MinTemp, weatherForecastCol3MinTemp];
        List<Image> futureWeatherImage = [futureWeatherCol0Image, futureWeatherCol1Image, futureWeatherCol2Image, futureWeatherCol3Image];
        
        for (int i = 0; i < 4; i++)
        {
            futureWeatherDate[i].Text = i == 0 ? "Tomorrow" : mainWindowUtils.getFutureDayName(i + 1);
            futureWeatherImage[i].Source = new Bitmap(AssetLoader.Open(new Uri($"avares://Shob3rsWeatherApp/Assets/Images/{mainWindowUtils.getWeatherImageName(currentWeather.futureWeatherDescriptions.ElementAt(i))}.png")));
            futureMaxTemp[i].Text = $"{currentWeather.futureHighs.ElementAt(i)}\u00b0{currentWeather.tempUnit}";
            futureMinTemp[i].Text = $"{currentWeather.futureLows.ElementAt(i)}\u00b0{currentWeather.tempUnit}";
        }
    }

    private void setWidgetContent()
    {
        // Day completion Widget
        dayProgressBar.Value = mainWindowUtils.calculatePercentageOfDayPassed();
        // Barometric Pressure Widget
        barPressure.Text = $"{currentWeather.airPressure.ToString(CultureInfo.CurrentCulture)} bar";
        // Humidity Segment Widget
        humidity.Text = $"{currentWeather.humidity}%";
        // Wind speed Widget
        windSpeed.Text = $"{currentWeather.windSpeed} {mainWindowUtils.getSpeedUnits()}";
        // Temp Lows Widget
        tempLows.Text = $"{currentWeather.minimumTemp}\u00b0{currentWeather.tempUnit}";
        // Temp Highs Widget
        tempHighs.Text = $"{currentWeather.maximumTemp}\u00b0{currentWeather.tempUnit}";
        // Currently Feels like Widget
        feelsLike.Text = $"{currentWeather.feelsLike}\u00b0{currentWeather.tempUnit}";
    }

    private void refreshWeatherButton(object? sender, RoutedEventArgs e)
    {
        // Restart the task that runs when the app launches
        setContentTask = setMenuContent();
    }

    private void openGithub(object? sender, RoutedEventArgs e)
    {
        Process.Start(new ProcessStartInfo { FileName = "https://github.com/Shob3r/Shob3rsWeatherApp", UseShellExecute = true });
    }
}
