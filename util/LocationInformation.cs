using System;
using System.IO;
using System.Threading.Tasks;
using IpData;

namespace Shob3rsWeatherApp;

public class LocationInformation
{
    private float? latitude, longitude;
    private string? currentCity, publicIpAddress;
    
    public LocationInformation()
    {
        try
        {
            var jsonParser = new JsonParser(File.ReadAllText("../../../config.json"));
            publicIpAddress = Task.Run(() => HttpUtils.getHttpContent("https://api.ipify.org")).Result;
            
            Task.Run(() => setLocationData(new IpDataClient(jsonParser.getDataByTag<string>("ipDataKey")), publicIpAddress));
        }
        catch (FileNotFoundException e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private async Task setLocationData(IpDataClient client, string ipAddress)
    {
        var fullIpInformation = await client.Lookup(ipAddress);
        currentCity = fullIpInformation.City;
        latitude = (float) fullIpInformation.Latitude!;
        longitude = (float) fullIpInformation.Longitude!;
    }
    
    public float getLatitude()
    {
        return latitude ?? throw new NullReferenceException();
    }

    public float getLongitude()
    {
        return longitude ?? throw new NullReferenceException();
    }

    public string? getCity()
    {
        return currentCity;
    }

    public string? getIpAddress()
    {
        return publicIpAddress;
    }
}