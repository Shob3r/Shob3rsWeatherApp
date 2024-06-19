using System;
using Newtonsoft.Json.Linq;

namespace Shob3rsWeatherApp;

public class JsonParser(string contents)
{
    // Stole this code from a different C# project I am working on
    private readonly JObject parsedFileContents = JObject.Parse(contents);

    public T getDataByTag<T>(string tagName, bool enableLogging = false)
    {
        if (doesTagExist(tagName))
        {
            if (enableLogging) Console.WriteLine("Found tag " + tagName);

            var token = parsedFileContents.SelectToken(tagName) ?? throw new InvalidOperationException();
            if (token.Type != JTokenType.Null)
            {
                var t = token.Value<T>();
                if (t != null) return t;
            }
        }

        Console.WriteLine($"WARNING: Did not find tag {tagName}");
        return default!;
    }

    private bool doesTagExist(string tagName)
    {
        var token = parsedFileContents.SelectToken(tagName);
        return token != null && token.Type != JTokenType.Null;
    }
}
