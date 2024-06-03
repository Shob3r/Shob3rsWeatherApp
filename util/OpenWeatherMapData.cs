using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace Shob3rsWeatherApp;

public class OpenWeatherMapData
{
    private readonly bool isUserAmerican;
    public OpenWeatherMapData()
    {
        var weatherMapKeyGetter = new JsonParser(File.ReadAllText("../../../config.json"));
        CultureInfo currentCulture = CultureInfo.CurrentCulture;
        if (currentCulture.Name.Equals("en_US", StringComparison.InvariantCultureIgnoreCase))
        {
            isUserAmerican = true;
        }
        
        Task.Run(() => getAllUsedWeatherInformation(weatherMapKeyGetter.getDataByTag<string>("openWeatherKey")));
    }

    private async Task getAllUsedWeatherInformation(string openWeatherMapKey)
    {
        // Regular Weather Section
        var locationInformation = new LocationInformation();
        var weatherRightNow = new JsonParser(await HttpUtils.getHttpContent($"https://api.openweathermap.org/data/2.5/weather?lat={locationInformation.getLatitude()}&lon={locationInformation.getLongitude()}&appid={openWeatherMapKey}"));
    }

    private int adjustTemperature(float unadjustedTemp)
    {
        if (isUserAmerican) 
            return (int)Math.Round((unadjustedTemp - 273) * 9 / 5 + 32);
        return (int) Math.Round(unadjustedTemp - 273);
    }
}