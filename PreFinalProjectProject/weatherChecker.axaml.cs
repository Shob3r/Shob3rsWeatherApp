using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace PreFinalProjectProject;

public partial class weatherChecker : Window
{
    private string weatherInCity;
    
    public weatherChecker()
    {
        weatherInCity = getCityText();
        InitializeComponent();
    }

    public string getCityText()
    {
        return "this is a test";
    }
    
    public string WeatherTextBinding
    {
        get => weatherInCity;
    }
}