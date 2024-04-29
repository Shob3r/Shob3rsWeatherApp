using System;
using System.IO;
using Newtonsoft.Json.Linq;

namespace PreFinalProjectProject;

public class JsonParser
{ 
    private JObject parsedFileContents;
    public JsonParser()
    {
        parsedFileContents = JObject.Parse(File.ReadAllText("../../../config.json"));
    }

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