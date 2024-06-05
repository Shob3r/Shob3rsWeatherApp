using System;
using Newtonsoft.Json.Linq;

namespace Shob3rsWeatherApp;

public class JsonParser(string contents)
{ 
    private JObject parsedFileContents = JObject.Parse(contents);

    public T getDataByTag<T>(string tagName, bool enableDebug = false)
    {
        if (doesTagExist(tagName))
        {
            if(enableDebug) Console.WriteLine("Found tag " + tagName);
            
            JToken token = parsedFileContents.SelectToken(tagName) ?? throw new InvalidOperationException();
            if (token.Type != JTokenType.Null)
            {
                T? t = token.Value<T>();
                if (t != null) return t;
            }
        }
        Console.WriteLine($"WARNING: Did not find tag {tagName}");
        return default!;
    }

    private bool doesTagExist(string tagName)
    {
        JToken? token = parsedFileContents.SelectToken(tagName);
        return token != null && token.Type != JTokenType.Null;
    }
}