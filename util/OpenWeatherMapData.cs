using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace Shob3rsWeatherApp;

public class OpenWeatherMapData
{
    public string sunsetTime, sunriseTime;
    private readonly LocationInformation locationInfo;

    private readonly bool isUserAmerican;
    private readonly string openWeatherMapKey;


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
        JsonParser weatherRightNowParser = new JsonParser(weatherRightNowInfo);
        
        int timezone = weatherRightNowParser.getDataByTag<int>("timezone");
        sunriseTime = normalizeUnixTime(weatherRightNowParser.getDataByTag<long>("sys.sunrise") + timezone);
        sunsetTime = normalizeUnixTime(weatherRightNowParser.getDataByTag<long>("sys.sunset") + timezone);
        
        // string weatherNextFiveDays = await HttpUtils.getHttpContent($"https://api.openweathermap.org/data/2.5/forecast?lat={locationInfo.latitude}&lon={locationInfo.longitude}&units={units}&appid={openWeatherMapKey}");
        //  futureWeatherDataParser = new JsonParser(weatherNextFiveDays);
    }

    private string getUnitType()
    {
        return isUserAmerican switch
        {
            true => "imperial",
            false => "metric"
        };
    }

    private float convertSpeed(float input)
    {
        if (isUserAmerican) return input;
        return input * 3.6f;
    }

    private string degreeToDirection(float degrees)
    {
        
    }
    
    private string normalizeUnixTime(long inputTime)
    {
        DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(inputTime);
        DateTime dateTime = dateTimeOffset.DateTime;

        return dateTime.ToString("hh:mm tt").ToLower();
    }
}
