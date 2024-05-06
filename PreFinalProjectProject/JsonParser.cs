using System;
using System.IO;
using Newtonsoft.Json.Linq;

namespace shobersWeatherApp;

public class JsonParser(string contents)
{ 
    private JObject parsedFileContents = JObject.Parse(contents);

    public T getDataByTag<T>(string tagName)
    {
        if (doesTagExist(tagName))
        { 
            JToken token = parsedFileContents.SelectToken(tagName) ?? throw new InvalidOperationException();
            if (token.Type != JTokenType.Null)
            {
                T? t = token.Value<T>();
                if (t != null) return t;
            }
        }

        return default!;
    }

    private bool doesTagExist(string tagName)
    {
        string fileContents = parsedFileContents.ToString();
        return fileContents.Contains(tagName);
    }
}