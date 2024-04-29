namespace PreFinalProjectProject;

public class WeatherConnector
{
    private string apiKey;
    private readonly JsonParser jsonParser = new JsonParser();
    public WeatherConnector()
    {
        apiKey = jsonParser.getDataByTag<string>("openWeatherKey");
    }

    public void getWeahterData()
    {

    }
}