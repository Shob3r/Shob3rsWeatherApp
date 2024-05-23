using System;
using System.IO;
using System.Reactive;
using ReactiveUI;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace PreFinalProjectProject;
public partial class MainWindow : Window
{
    private LocationData locationData = new LocationData();
    private JsonParser openWeatherMapKey = new JsonParser(File.ReadAllText("../../../config.json"));
    
    
    public MainWindow()
    {
        InitializeComponent();
    }

    public void CloseMenu(object message)
    {
        Console.WriteLine(message);
    }
    
    private int toCelsius(float kelvin)
    {  
        return (int)kelvin - 273;
    }  

    private int toFahrenheit(float kelvin)
    {
        // Convert to C to make it easier
        int celsius = toCelsius(kelvin);
        return (int)Math.Round((double)(celsius * (9 / 5) + 32));
    }

    private void ToggleMenu()
    {
        
    }
    protected override async void OnLoaded(RoutedEventArgs routedEventArgs)
    {
        
        base.OnLoaded(routedEventArgs);
    }


    private void CloseMenu(object? sender, RoutedEventArgs e)
    {
        sideMenu.IsPaneOpen = !sideMenu.IsPaneOpen;
    }

    private void openMainMenu(object? sender, RoutedEventArgs e)
    {
        Console.WriteLine("Clicked!");
    }
}
