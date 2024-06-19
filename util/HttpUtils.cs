using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Shob3rsWeatherApp.Util;

public static class HttpUtils
{
    public static async Task<string> getHttpContent(string url, bool enableLogging = false)
    {
        if(enableLogging) Console.WriteLine(url);

        using HttpClient httpClient = new HttpClient();
        try
        {
            HttpResponseMessage response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine(e);
            return string.Empty;
        }
    }
}
