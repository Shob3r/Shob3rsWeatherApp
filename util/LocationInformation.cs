using System;
using System.IO;
using System.Threading.Tasks;
using IpData;
using Shob3rsWeatherApp.Util;

namespace Shob3rsWeatherApp;

public class LocationInformation
{
    private IpDataClient client;
    
    public float? latitude, longitude;
    public string? currentCity, publicIpAddress;
    
    public LocationInformation()
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
    }

    public async Task setLocationData()
    {
        var fullIpInformation = await client.Lookup(publicIpAddress);
        currentCity = fullIpInformation.City;
        latitude = (float) fullIpInformation.Latitude!;
        longitude = (float) fullIpInformation.Longitude!;
    }
}
