using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Shob3rsWeatherApp.Util;

namespace Shob3rsWeatherApp;

public class OpenWeatherMapData
{
    public string? sunsetTime, sunriseTime, tempUnit, weatherDescription, detailedWeatherDescription;
    public int tempNow, feelsLike, minimumTemp, maximumTemp;
    public float airPressure, windSpeed;
    
    private readonly bool isUserAmerican;
    private readonly string openWeatherMapKey, customCityName;
    
    private LocationInformation locationInfo;
    
    public OpenWeatherMapData(string customCityName = "")
    {
        JsonParser weatherMapKeyGetter = new JsonParser(File.ReadAllText("../../../config.json"));
        locationInfo = new LocationInformation();
        openWeatherMapKey = weatherMapKeyGetter.getDataByTag<string>("openWeatherKey");

        CultureInfo currentCulture = CultureInfo.CurrentCulture;
        isUserAmerican = currentCulture.Name.Equals("en_US", StringComparison.InvariantCultureIgnoreCase);
        tempUnit = isUserAmerican ? "F" : "C";

        this.customCityName = customCityName;
        Task.Run(setWeatherData);
    }
    
    public async Task setWeatherData()
    {
        await locationInfo.setLocationData();
        
        string units = getUnitType();
        string url;
        url = customCityName != "" ? $"https://api.openweathermap.org/data/2.5/weather?q={customCityName}&units={units}&appid={openWeatherMapKey}" : $"https://api.openweathermap.org/data/2.5/weather?q={locationInfo.currentCity}&units={units}&appid={openWeatherMapKey}";
        
        Console.WriteLine(url);
        string weatherRightNowInfo = await HttpUtils.getHttpContent(url);
        JsonParser weatherParser = new JsonParser(weatherRightNowInfo);
        
        int timezone = weatherParser.getDataByTag<int>("timezone");
        // Add the timezone variable to whatever the unix time is to adjust it to the timezone the user is currently in. without it, we'd be using the UTC time in greenwich 
        
        sunriseTime = normalizeUnixTime(weatherParser.getDataByTag<long>("sys.sunrise") + timezone);
        sunsetTime = normalizeUnixTime(weatherParser.getDataByTag<long>("sys.sunset") + timezone);

        weatherDescription = weatherParser.getDataByTag<string>("weather[0].main");
        detailedWeatherDescription = weatherParser.getDataByTag<string>("weather[0].description");
        tempNow = roundTemp(weatherParser.getDataByTag<float>("main.temp"));
        feelsLike = roundTemp(weatherParser.getDataByTag<float>("main.feels_like"));
        maximumTemp = roundTemp(weatherParser.getDataByTag<float>("main.temp_min"));
        minimumTemp = roundTemp(weatherParser.getDataByTag<float>("main.temp_max"));
        airPressure = convertAirPressure(weatherParser.getDataByTag<int>("main.pressure"));
    }
    
    private float convertSpeed(float input)
    {
        if (isUserAmerican) return input;
        return input * 3.6f; // to convert m/s to km/h, multiply m/s by 3.6
    }

    private int roundTemp(float temperatureBeforeRounding)
    {
        // Convert the value that OpenWeatherMap provides (which contains decimal places) into an integer, while rounding up or down to the nearest number before converting
        return (int)Math.Round(temperatureBeforeRounding);
    }
    
    private float convertAirPressure(int pressure)
    {
        // I'll just use bar because that's the one that makes the most sense
        // openWeatherMap returns Hpa (hectopascal). 1hpa = 0.001 bar
        return pressure * 0.001f;
    }

    private string normalizeUnixTime(long inputTime)
    {
        DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(inputTime);
        DateTime dateTime = dateTimeOffset.DateTime;

        return dateTime.ToString("hh:mm tt").ToLower(); // Using ToLower() here as without it, am/pm will be all capitalized
    }
    
    private string getUnitType()
    {
        return isUserAmerican ? "imperial" : "metric";
    }
}
