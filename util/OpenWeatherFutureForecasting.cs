using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Shob3rsWeatherApp.Util;

namespace Shob3rsWeatherApp;

public class OpenWeatherFutureForecasting : OpenWeatherData
{
    public List<string>? futureTemperatures = [];
    public List<string>? futureWeatherDescriptions = [];

    public override async Task setWeatherData()
    {
        // TODO: Migrate to OneCall V3 for future forecasting if I have the time for it
        JsonParser weatherParser = new JsonParser(await HttpUtils.getHttpContent($"https://api.openweathermap.org/data/2.5/forecast?q={LocationInformation.currentCity},{LocationInformation.countryOfResidence}&units={getUnitType()}&appid={openWeatherMapKey}"));

        // Cursed for loop conditions
        for (int i = 7; i < 40; i += 8)
        {
            // When asking OpenWeatherMap API v2.5 about the weather in the future, it returns weather data in the future in three hour increments.
            // I only want to show the weather once every 24 hours, and start 24 hours from now.
            // So we start at element 7 of the array, which is 24 hours from when the application runs, and then increment it by 8 each time from there on
            // As 8 segments of 3 hour increments = 24 hours
            // A little difficult to explain for 1am me but it makes sense when it's thought about 
            
            try
            {
                futureTemperatures?.Add(roundTemp(weatherParser.getDataByTag<float>($"list[{i}].main.temp")).ToString());
                futureWeatherDescriptions?.Add(weatherParser.getDataByTag<string>($"list[{i}].weather[0].main"));
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine($"oops\n{e}");
                throw;
            }
        }
    }
}