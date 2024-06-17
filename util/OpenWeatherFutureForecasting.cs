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
        string units = getUnitType();
        Debug.Assert(LocationInformation.currentCity != null || LocationInformation.currentCity != "");
        Debug.Assert(LocationInformation.countryOfResidence != null || LocationInformation.countryOfResidence != "");

        string url =
            $"https://api.openweathermap.org/data/2.5/forecast?q={LocationInformation.currentCity},{LocationInformation.countryOfResidence}&units={units}&appid={openWeatherMapKey}";

        string futureWeatherInfo = await HttpUtils.getHttpContent(url);
        JsonParser weatherParser = new(futureWeatherInfo); // WHAT IS THIS BLACK MAGIC

        // For now, I only want to know the temp in 24 hours for the next 5 days, so it's time to do some silly json shenanigans
        // Cursed for loop conditions
        for (int i = 7; i < 40; i += 8)
            try
            {
                futureTemperatures?.Add(roundTemp(weatherParser.getDataByTag<float>($"list[{i}].main.temp")).ToString());
                futureWeatherDescriptions?.Add(weatherParser.getDataByTag<string>($"list[{i}].weather[0].main"));
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("oops");
                throw;
            }
    }
}