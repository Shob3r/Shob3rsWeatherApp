using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using OpenWeatherMap;
using OpenWeatherMap.Util;

namespace Shob3rsWeatherApp;

public class OpenWeatherMapData
{
    private readonly bool isUserAmerican;
    private readonly string openWeatherMapKey;
    private LocationInformation locationInfo;
    
    public int tempurature;
    
    public OpenWeatherMapData()
    {
        // Instantiate LocationInformation here to prevent it from not having the data I need when the http request for weather data executes
        locationInfo = new LocationInformation();
        JsonParser weatherMapKeyGetter = new JsonParser(File.ReadAllText("../../../config.json"));
        openWeatherMapKey = weatherMapKeyGetter.getDataByTag<string>("openWeatherKey");
        
        CultureInfo currentCulture = CultureInfo.CurrentCulture;
        isUserAmerican = currentCulture.Name.Equals("en_US", StringComparison.InvariantCultureIgnoreCase);
        Task.Run(setWeatherData);
    }

    public async Task setWeatherData()
    {
        await locationInfo.setLocationData();
        string units = getUnitType();
        string weatherRightNowInfo = await HttpUtils.getHttpContent($"https://api.openweathermap.org/data/2.5/weather?lat={locationInfo.latitude}&lon={locationInfo.longitude}&units={units}&appid={openWeatherMapKey}");
        JsonParser weatherDataParser = new JsonParser(weatherRightNowInfo);


        string weatherNextFiveDays = await HttpUtils.getHttpContent($"https://api.openweathermap.org/data/2.5/forecast?lat={locationInfo.latitude}&lon={locationInfo.longitude}&units={units}&appid={openWeatherMapKey}");
        Console.WriteLine(weatherNextFiveDays);
        JsonParser futureWeatherDataParser = new JsonParser(weatherNextFiveDays);
    }

    private string getUnitType()
    {
        return isUserAmerican switch
        {
            true => "imperial",
            false => "metric"
        };
    }

    private float metersPerSecondToKilometersPerHour(float input)
    {
        if (isUserAmerican) return input;
        return input * 3.6f;
    }
}
