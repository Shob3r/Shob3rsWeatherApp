using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Shob3rsWeatherApp.Util;

namespace Shob3rsWeatherApp;

public class OpenWeatherData
{
    public readonly bool isUserAmerican;
    public readonly string tempUnit;
    
    public int tempNow, feelsLike, minimumTemp, maximumTemp;
    public float airPressure, windSpeed, humidity;
    public string weatherDescription, detailedWeatherDescription, weatherOutlook;
    
    public readonly List<string> futureHighs = [];
    public readonly List<string> futureLows = [];
    public readonly List<string> futureWeatherDescriptions = [];

    public OpenWeatherData()
    {
        CultureInfo currentCulture = CultureInfo.CurrentCulture;
        isUserAmerican = currentCulture.Name.Equals("en_US", StringComparison.InvariantCultureIgnoreCase);
        tempUnit = isUserAmerican ? "F" : "C";
    }

    public async Task updateWeatherData()
    {
        JsonParser weatherParser = new JsonParser(await HttpUtils.getHttpContent($"https://api.openweathermap.org/data/3.0/onecall?lat={LocationInformation.latitude}&lon={LocationInformation.longitude}&exclude=minutely,hourly&units={getMeasurementSystem()}&appid={Env.openWeatherKey}"));
        
        updateWeatherDescriptions(weatherParser);
        updateWeatherForecasts(weatherParser);
        updateTempData(weatherParser);
        updateMiscData(weatherParser);
    }

    private void updateWeatherDescriptions(JsonParser weatherParser)
    {
        weatherDescription = weatherParser.getDataByTag<string>("current.weather[0].main");
        detailedWeatherDescription = weatherParser.getDataByTag<string>("current.weather[0].description");
        weatherOutlook = weatherParser.getDataByTag<string>("daily[0].summary");
    }
    
    private void updateTempData(JsonParser weatherParser)
    {
        tempNow = roundTemp(weatherParser.getDataByTag<float>("current.temp"));
        feelsLike = roundTemp(weatherParser.getDataByTag<float>("current.feels_like"));
        minimumTemp = roundTemp(weatherParser.getDataByTag<float>("daily[0].temp.min"));
        maximumTemp = roundTemp(weatherParser.getDataByTag<float>("daily[0].temp.max"));
    }

    private void updateMiscData(JsonParser weatherParser)
    {
        airPressure = convertAirPressure(weatherParser.getDataByTag<int>("current.pressure"));
        humidity = weatherParser.getDataByTag<float>("current.humidity");
        windSpeed = convertSpeed(weatherParser.getDataByTag<float>("current.wind_speed"));
    }

    private void updateWeatherForecasts(JsonParser weatherParser)
    {
        for (int i = 0; i < 4; i++)
        {
            futureHighs.Add(roundTemp(weatherParser.getDataByTag<float>($"daily[{i}].temp.max")).ToString());
            futureLows.Add(roundTemp(weatherParser.getDataByTag<float>($"daily[{i}].temp.min")).ToString());
            
            futureWeatherDescriptions.Add(weatherParser.getDataByTag<string>($"daily[{i}].weather[0].main"));
        }
    }
    
    private float convertSpeed(float input)
    {
        // To convert m/s to km/h, multiply m/s by 3.6
        // Also I LOVE TERNARY EXPRESSIONS
        return isUserAmerican ? input : (float)Math.Round(input * 3.6f, 1); // Round to the nearest tenth
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
        return (float)Math.Round(pressure * 0.001f, 2); // Round to nearest hundredth
    }

    protected string getMeasurementSystem()
    {
        return isUserAmerican ? "imperial" : "metric";
    }
}