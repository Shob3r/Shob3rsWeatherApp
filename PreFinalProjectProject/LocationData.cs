using System;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using IpData;

namespace PreFinalProjectProject;

public class LocationData
{
    private string? ipAddress;
    private readonly string ipDataKey;

    public LocationData()
    {
        JsonParser parser = new JsonParser();
        ipDataKey = parser.getDataByTag<string>("ipDataKey");
        
        // Async methods are so fun to deal with!
        ipAddress = Task.Run(() => getHttpContent("https://api.ipify.org")).Result;
    }

    private async Task<string> getHttpContent(string url)
    {
        using var httpClient = new HttpClient();
        try
        {
            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();
            Console.WriteLine(content);
            
            return content;
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine(e);
            return string.Empty;
        }
    }

    private async Task setCoordinates()
    {
        var client = new IpDataClient(ipDataKey);
        var ipInfo = await client.Lookup(ipAddress);
        
        Console.WriteLine($"Latitude: {ipInfo.Latitude}. Longitude: {ipInfo.Longitude}");
    }
    
    public string GetIpAddress()
    {
        return ipAddress;
    }
}