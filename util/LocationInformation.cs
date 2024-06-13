using System;
using System.IO;
using System.Threading.Tasks;
using IpData;
using Shob3rsWeatherApp.Util;

namespace Shob3rsWeatherApp;

public static class LocationInformation
{
    public static float? latitude, longitude;
    public static string? currentCity, publicIpAddress;
    
    private static IpDataClient client;
    private static int callCount = 0;

    public static async Task setLocationData()
    {
        try
        {
            var jsonParser = new JsonParser(File.ReadAllText("../../../config.json"));
            publicIpAddress = Task.Run(() => HttpUtils.getHttpContent("https://api.ipify.org")).Result;
            client = new IpDataClient(jsonParser.getDataByTag<string>("ipDataKey"));
        }
        catch (FileNotFoundException e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        if (callCount == 0)
        {
            Console.WriteLine("Setting location info...");
            callCount++;
            var fullIpInformation = await client.Lookup(publicIpAddress);
            currentCity = fullIpInformation.City;
            latitude = (float) fullIpInformation.Latitude!;
            longitude = (float) fullIpInformation.Longitude!;
        }
    }
}
