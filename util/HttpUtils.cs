using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Shob3rsWeatherApp.Util;

public static class HttpUtils
{
    public static async Task<string> getHttpContent(string url)
    {
        Console.WriteLine(url);

        using var httpClient = new HttpClient();
        try
        {
            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();

            return content;
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine(e);
            return string.Empty;
        }
    }
}