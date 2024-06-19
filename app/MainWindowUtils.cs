using System;

namespace Shob3rsWeatherApp;

public class MainWindowUtils(OpenWeatherData currentWeather) // futureForecast not used for now but may be needed in the future
{
    public string getWeatherImageName(string inputWeatherDescription)
    {
        // This returns exact file names for the image names (without png)
        return inputWeatherDescription.ToLower() switch
        {
            "clear" => $"clear-{getTime(true)}",
            "drizzle" => "rainy",
            "rain" => "storm",
            "snow" => "snowy",
            "mist" => "overcast",
            "haze" => "overcast",
            "fog" => "fog",
            "ash" => "fog",
            "squall" => "windy",
            "tornado" => "tornado",
            "clouds" => $"cloudy-{getTime(true)}",
            _ => "unknown"
        };
    }

    public string getFutureDayName(int days)
    {
        DateTime thePresent = DateTime.Today;
        DateTime futureDate = thePresent.AddDays(days);

        return futureDate.DayOfWeek.ToString();
    }

    public string getTime(bool onlyDayAndNight = false)
    {
        DateTime currentTime = DateTime.Now;
        int currentHour = currentTime.Hour;

        if (onlyDayAndNight) // For other methods that only want day and night stuff, rather than the default return value
            return currentHour switch
            {
                >= 6 and < 18 => "day",
                _ => "night"
            };

        return currentHour switch
        {
            >= 6 and < 12 => "Morning",
            >= 12 and < 19 => "Afternoon",
            _ => "Evening"
        };
    }

    public string getSpeedUnits()
    {
        return currentWeather.isUserAmerican ? "Mph" : "Km/h";
    }

    public double calculatePercentageOfDayPassed()
    {
        // The only thing generated with ChatGPT in this whole project
        DateTime now = DateTime.Now;
        const double totalSecondsInDay = 24 * 60 * 60;
        double secondsElapsedToday = (now - now.Date).TotalSeconds;
        double percentageCompleted = secondsElapsedToday / totalSecondsInDay * 100;
        return percentageCompleted;
    }
}