using System.Collections.Generic;
using System.Threading.Tasks;
using Shob3rsWeatherApp.Util;

namespace Shob3rsWeatherApp;
public class OpenWeatherFutureForecasting : OpenWeatherData
{
    public readonly List<string> futureTemperatures = [];
    public readonly List<string> futureWeatherDescriptions = [];

    public override async Task updateWeatherData()
    {
        JsonParser weatherParser = new JsonParser(await HttpUtils.getHttpContent($"https://api.openweathermap.org/data/3.0/onecall?lat={LocationInformation.latitude}&lon={LocationInformation.longitude}&exclude=current,minutely,hourly,alerts&units=metric&appid={Env.openWeatherKey}"));

        for (int i = 0; i < 4; i++)
        {
            futureTemperatures.Add(roundTemp(weatherParser.getDataByTag<float>($"daily[{i}].temp.max")).ToString());
            futureWeatherDescriptions.Add(weatherParser.getDataByTag<string>($"daily[{i}].weather[0].main"));
        }
    }
}
