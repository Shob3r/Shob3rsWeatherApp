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
    private MainWindowUtils mainWindowUtils;
    private readonly OpenWeatherData currentWeather;
    private readonly OpenWeatherFutureForecasting futureForecast;
    private readonly TextInfo textInfo = new CultureInfo("en-CA", false).TextInfo;

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
        // Create a text formatter, so I can use ToTileCase() when I need to, then wait for the classes to set their data, so this method can execute properly
        await LocationInformation.setLocationData();
        await futureForecast.setWeatherData();
        await currentWeather.setWeatherData();
        mainWindowUtils = new MainWindowUtils(currentWeather, futureForecast);

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
        if (currentWeather.detailedWeatherDescription != null) weatherDescription.Text = textInfo.ToTitleCase(currentWeather.detailedWeatherDescription);
        todaysWeather.Text = currentWeather.todaysWeatherDescription;
    }

    private void setForecastContent()
    {
        List<TextBlock> futureWeatherDate = [futureWeatherCol0Date, futureWeatherCol1Date, futureWeatherCol2Date, futureWeatherCol3Date];
        List<TextBlock> futureWeatherTemp = [futureWeatherCol0Temp, futureWeatherCol1Temp, futureWeatherCol2Temp, futureWeatherCol3Temp];
        List<Image> futureWeatherImage = [futureWeatherCol0Image, futureWeatherCol1Image, futureWeatherCol2Image, futureWeatherCol3Image];
        for (int i = 0; i < 4; i++)
        {
            futureWeatherDate[i].Text = i == 0 ? "Tomorrow" : mainWindowUtils.getFutureDayName(i + 1);
            futureWeatherImage[i].Source = new Bitmap(AssetLoader.Open(new Uri($"avares://Shob3rsWeatherApp/Assets/Images/{mainWindowUtils.getWeatherImageName(futureForecast.futureWeatherDescriptions!.ElementAt(i))}.png")));
            futureWeatherTemp[i].Text = $"{futureForecast.futureTemperatures!.ElementAt(i)}\u00b0{currentWeather.tempUnit}";
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
}