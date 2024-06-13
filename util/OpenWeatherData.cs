using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Shob3rsWeatherApp.Util;

namespace Shob3rsWeatherApp;

public class OpenWeatherData
{
    private readonly bool isUserAmerican;
    protected readonly string openWeatherMapKey, customCityName;
    public readonly string? tempUnit;
    public float airPressure, windSpeed;
    public string? sunsetTime, sunriseTime;
    public int tempNow, feelsLike, minimumTemp, maximumTemp;

    public string? weatherDescription, detailedWeatherDescription;

    public OpenWeatherData(string customCityName = "")
    {
        var weatherMapKeyGetter = new JsonParser(File.ReadAllText("../../../config.json"));
        openWeatherMapKey = weatherMapKeyGetter.getDataByTag<string>("openWeatherKey");

        var currentCulture = CultureInfo.CurrentCulture;
        isUserAmerican = currentCulture.Name.Equals("en_US", StringComparison.InvariantCultureIgnoreCase);
        tempUnit = isUserAmerican ? "F" : "C";

        this.customCityName = customCityName;
    }

    public virtual async Task setWeatherData()
    {
        string units = getUnitType();
        string url = customCityName != ""
            ? $"https://api.openweathermap.org/data/2.5/weather?q={customCityName}&units={units}&appid={openWeatherMapKey}"
            : $"https://api.openweathermap.org/data/2.5/weather?q={LocationInformation.currentCity},{LocationInformation.countryOfResidence}&units={units}&appid={openWeatherMapKey}";

        string weatherRightNowInfo = await HttpUtils.getHttpContent(url);
        var weatherParser = new JsonParser(weatherRightNowInfo);

        int timezone = weatherParser.getDataByTag<int>("timezone");

        // Add the timezone variable to whatever the unix time is to adjust it to the timezone the user is currently in. without it, we'd be using UTC time
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

    protected float convertSpeed(float input)
    {
        if (isUserAmerican) return input;
        return input * 3.6f; // to convert m/s to km/h, multiply m/s by 3.6
    }

    protected int roundTemp(float temperatureBeforeRounding)
    {
        // Convert the value that OpenWeatherMap provides (which contains decimal places) into an integer, while rounding up or down to the nearest number before converting
        return (int)Math.Round(temperatureBeforeRounding);
    }

    protected float convertAirPressure(int pressure)
    {
        // I'll just use bar because that's the one that makes the most sense
        // openWeatherMap returns Hpa (hectopascal). 1hpa = 0.001 bar
        return pressure * 0.001f;
    }

    protected string normalizeUnixTime(long inputTime)
    {
        var dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(inputTime);
        var dateTime = dateTimeOffset.DateTime;

        return
            dateTime.ToString("hh:mm tt")
                .ToLower(); // Using ToLower() here as without it, am/pm will be all capitalized
    }

    protected string getUnitType()
    {
        return isUserAmerican ? "imperial" : "metric";
    }
}