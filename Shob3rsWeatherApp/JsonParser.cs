using System;
using Newtonsoft.Json.Linq;

namespace Shob3rsWeatherApp;

public class JsonParser(string contents)
{ 
    private JObject parsedFileContents = JObject.Parse(contents);

    public T getDataByTag<T>(string tagName)
    {
        if (doesTagExist(tagName))
        { 
            Console.WriteLine("Found tag " + tagName);
            JToken token = parsedFileContents.SelectToken(tagName) ?? throw new InvalidOperationException();
            if (token.Type != JTokenType.Null)
            {
                T? t = token.Value<T>();
                if (t != null) return t;
            }
        }
        Console.WriteLine($"Did not find tag {tagName}");
        return default!;
    }

    private bool doesTagExist(string tagName)
    {
        string fileContents = parsedFileContents.ToString();
        return fileContents.Contains(tagName);
    }
}