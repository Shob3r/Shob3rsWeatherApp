using System;
using System.Globalization;
using System.Threading.Tasks;
using Shob3rsWeatherApp.Util;

namespace Shob3rsWeatherApp;

public class OpenWeatherData
{
    public readonly bool isUserAmerican;
    protected readonly string openWeatherMapKey;
    public readonly string? tempUnit;
    public float airPressure, windSpeed, humidity;

    public int tempNow, feelsLike, minimumTemp, maximumTemp;

    public string? weatherDescription, detailedWeatherDescription, todaysWeatherDescription;

    public OpenWeatherData()
    {
        openWeatherMapKey = Env.openWeatherKey;

        var currentCulture = CultureInfo.CurrentCulture;
        isUserAmerican = currentCulture.Name.Equals("en_US", StringComparison.InvariantCultureIgnoreCase);
        tempUnit = isUserAmerican ? "F" : "C";
    }

    public virtual async Task setWeatherData()
    {
        string units = getUnitType();
        string url =
            $"https://api.openweathermap.org/data/3.0/onecall?lat={LocationInformation.latitude}&lon={LocationInformation.longitude}&exclude=minutely,hourly&units={getUnitType()}&appid={Env.openWeatherKey}";

        string weatherRightNowInfo = await HttpUtils.getHttpContent(url);
        var weatherParser = new JsonParser(weatherRightNowInfo);

        weatherDescription = weatherParser.getDataByTag<string>("current.weather[0].main");
        detailedWeatherDescription = weatherParser.getDataByTag<string>("current.weather[0].description");
        tempNow = roundTemp(weatherParser.getDataByTag<float>("current.temp"));
        feelsLike = roundTemp(weatherParser.getDataByTag<float>("current.feels_like"));
        maximumTemp = roundTemp(weatherParser.getDataByTag<float>("daily[0].temp.min"));
        minimumTemp = roundTemp(weatherParser.getDataByTag<float>("daily[0].temp.max"));
        todaysWeatherDescription = weatherParser.getDataByTag<string>("daily[0].summary");
        airPressure = convertAirPressure(weatherParser.getDataByTag<int>("current.pressure"));
        humidity = weatherParser.getDataByTag<float>("current.humidity");
        windSpeed = convertSpeed(weatherParser.getDataByTag<float>("current.wind_speed"));
    }

    private float convertSpeed(float input)
    {
        if (isUserAmerican) return input;
        return (float)Math.Round(input * 3.6f, 1); // to convert m/s to km/h, multiply m/s by 3.6
    }

    protected int roundTemp(float temperatureBeforeRounding)
    {
        // Convert the value that OpenWeatherMap provides (which contains decimal places) into an integer, while rounding up or down to the nearest number before converting
        return (int)Math.Round(temperatureBeforeRounding);
    }

    private float convertAirPressure(int pressure)
    {
        // I'll just use bar because that's the one that makes the most sense
        // openWeatherMap returns Hpa (hectopascal). 1hpa = 0.001 bar
        return (float)Math.Round(pressure * 0.001f, 2);
    }

    protected string getUnitType()
    {
        return isUserAmerican ? "imperial" : "metric";
    }

    private string normalizeUnixTime(long inputTime)
    {
        var dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(inputTime);
        var dateTime = dateTimeOffset.DateTime;

        return dateTime.ToString("hh:mm tt").ToUpper();
    }
}