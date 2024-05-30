using System;
using System.IO;
using System.Threading.Tasks;
using IpData;

namespace Shob3rsWeatherApp;

public class LocationData
{
    private string? ipAddress;
    private readonly string ipDataKey;
    private string city;
    
    public LocationData()
    {
        JsonParser configData = new JsonParser(File.ReadAllText("../../config.json"));
        ipDataKey = configData.getDataByTag<string>("ipDataKey");
        
        // Async methods are so fun to deal with!
        ipAddress = Task.Run(() => HttpUtils.getHttpContent("https://api.ipify.org")).Result;
    }
    
    public async Task SetLocationData()
    {
        var client = new IpDataClient(ipDataKey);
        var ipInfo = await client.Lookup(ipAddress);
        city = ipInfo.City ?? throw new InvalidOperationException();
        Console.WriteLine(city);
    }
}