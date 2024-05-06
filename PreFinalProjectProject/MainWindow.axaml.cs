using System;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace shobersWeatherApp;
public partial class MainWindow : Window
{
    private LocationData locationData = new LocationData();
    private Task setContentTask;
    public MainWindow()
    {
        InitializeComponent();
        setContentTask = setContent();
    }

    private async Task setContent()
    {
        await locationData.SetLocationData();
        cityText.Text = $"Weather in {locationData.city}:";

        var openWeatherClient = new OpenWeatherAPI.OpenWeatherApiClient("e9af5ddc7ce0e7e49997fc31170ec477");
        var fullWeatherQuery = await openWeatherClient.QueryAsync(locationData.city);
        
        Console.WriteLine($"Testing temp \n Temp is {fullWeatherQuery.WeatherList} and {fullWeatherQuery.Main.Temperature.CelsiusCurrent} ");
        
        // OpenWeatherMap gaming

    }
    
    protected override async void OnLoaded(RoutedEventArgs routedEventArgs)
    {
        await setContentTask;
        base.OnLoaded(routedEventArgs);
    }
}