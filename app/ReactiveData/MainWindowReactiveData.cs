using ReactiveUI;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Shob3rsWeatherApp;

public class MainWindowReactiveData : ReactiveObject
{
    private LocationInformation locationInfo = new LocationInformation();
    
    
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
}