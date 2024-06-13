using System.Collections.Generic;
using System.Threading.Tasks;
using Shob3rsWeatherApp.Util;

namespace Shob3rsWeatherApp;

public class OpenWeatherFutureForecasting(string customCityName = "") : OpenWeatherData(customCityName)
{
    public List<string>? futureTemperatures;
    public List<string>? futureWeatherDescriptions;

    public override async Task setWeatherData()
    {
        string units = getUnitType();
        string url = customCityName != ""
            ? $"https://api.openweathermap.org/data/2.5/forecast?q={customCityName}&units={units}&appid={openWeatherMapKey}"
            : $"https://api.openweathermap.org/data/2.5/forecast?q={LocationInformation.currentCity}&units={units}&appid={openWeatherMapKey}";

        string futureWeatherInfo = await HttpUtils.getHttpContent(url);
        var weatherParser = new JsonParser(futureWeatherInfo);


        // For now, I only want to know the temp in 24 hours for the next 5 days, so it's time to do some silly json shenanigans
        for (int i = 0; i < 4; i++)
        {
            // Data returned is a json array of future weather in three hour increments. Multiplying by 8 will get the weather in exactly 24 hours from now
            // For example, the first time this loop runs, 1 * 8 = 8, and the 8th pos in the json array that gets returned from the web request is 24 hours from onw
            // easy-peasy

            futureTemperatures?.Add(roundTemp(weatherParser.getDataByTag<float>($"list[{(i + 1) * 8}].main.temp"))
                .ToString());
            futureWeatherDescriptions?.Add(weatherParser.getDataByTag<string>($"list[{(i + 1) * 8}].weather[0].main"));
        }
    }
}