using ReactiveUI;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Shob3rsWeatherApp;

public class MainWindowReactiveData : ReactiveObject
{
    private LocationInformation locationInfo = new LocationInformation();
    private OpenWeatherMapData openWeatherMapData = new OpenWeatherMapData();

    public MainWindowReactiveData()
    {
        Task.Run(() => locationInfo.setLocationData());
    }
    
    // MVVM works in strange ways
    private string? _UserName;
    public string? UserName
    {
        get => _UserName;
        set
        {
            _UserName = value;  
            this.RaiseAndSetIfChanged(ref _UserName, Environment.UserName);
        }
    }

    private string? _CurrentTemp;

    public string? CurrentTemp
    {
        get => _CurrentTemp;
        set
        {
            _CurrentTemp = value;
            this.RaiseAndSetIfChanged(ref _CurrentTemp, getCurrentTemperature());
        }
    }
    
    private string getCurrentTemperature()
    {
        return "";
    }

    public string? _City;

    public string? City
    {
        get => _City;
        set
        {
            _City = value;
            this.RaiseAndSetIfChanged(ref _City, $"Refresh Data In {locationInfo.currentCity}");
        }
    }
}