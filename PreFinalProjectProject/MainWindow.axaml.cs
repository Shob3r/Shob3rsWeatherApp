using System;
using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using OpenWeatherMap;
using OpenWeatherMap.Util;

namespace PreFinalProjectProject;
public partial class MainWindow : Window
{
    private LocationData locationData = new LocationData();
    private JsonParser openWeatherMapKey = new JsonParser(File.ReadAllText("../../../config.json"));
    private Task setContentTask;
    
    public MainWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
        setContentTask = setContent();
    }
    
    private async Task setContent()
    {
        await locationData.SetLocationData();
        cityText.Text = $"Weather in {locationData.city}:";

        var openWeatherMap = new OpenWeatherMapService(new OpenWeatherMapOptions()
        {
            ApiKey = openWeatherMapKey.getDataByTag<string>("openWeatherKey")
        });
        RequestOptions.Default.Unit = UnitType.Metric;
        
        var weatherData = await openWeatherMap.GetCurrentWeatherAsync(locationData.city);
        weatherTesting.Text = $"{(int) weatherData.Temperature.Value} Degrees Celsius.";
    }

    public void setCityText()
    {
        
    }

    public void setWeatherText()
    {
        
    }
    
    private int toCelsius(float kelvin)
    {  
        return (int)kelvin - 273;
    }  

    private int toFahrenheit(float kelvin)
    {
        // Convert to C to make it easier
        int celsius = toCelsius(kelvin);
        return (int)Math.Round((double)(celsius * (9 / 5) + 32));
    }
    
    protected override async void OnLoaded(RoutedEventArgs routedEventArgs)
    {
        await setContentTask;
        base.OnLoaded(routedEventArgs);
    }
}
