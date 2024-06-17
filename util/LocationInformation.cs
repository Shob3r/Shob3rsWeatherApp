using System;
using System.IO;
using System.Threading.Tasks;
using IpData;
using Shob3rsWeatherApp.Util;

namespace Shob3rsWeatherApp;

public static class LocationInformation
{
    public static float? latitude, longitude;
    public static string currentCity, countryOfResidence, fullCountryName, publicIpAddress;

    private static IpDataClient client;
    private static int callCount;

    public static async Task setLocationData()
    {
        try
        {
            publicIpAddress = Task.Run(() => HttpUtils.getHttpContent("https://api.ipify.org")).Result; // This website is a life-saver
            client = new IpDataClient(Env.ipDataKey);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        if (callCount == 0)
        {
            // Prevent IPData from calling more than once and eating through the daily api call limit
            callCount++;
            var fullIpInformation = await client.Lookup(publicIpAddress);
            currentCity = fullIpInformation.City;
            latitude = (float)fullIpInformation.Latitude!;
            longitude = (float)fullIpInformation.Longitude!;
            countryOfResidence = fullIpInformation.CountryCode;
            fullCountryName = fullIpInformation.CountryName;
        }
    }
}
