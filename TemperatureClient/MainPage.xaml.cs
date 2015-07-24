using Microsoft.AspNet.SignalR.Client;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TemperatureClient
{
    /*
     * This class controls the logic behind the varoius components in MainPage's XAML file
     */
    public sealed partial class MainPage : Page
    {
        // A client side proxy for a server side hub
        IHubProxy hub;


        // This constructor is called when MainPage is initialized
        public MainPage()
        {
            this.InitializeComponent();
            // Registers MainPage_Loaded() as an event handler method that runs, when the event "this.Loaded" is triggered 
            this.Loaded += MainPage_Loaded;
        }


        // An event handler that runs on successful page load
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            // Prepares a SignalRClient object for this app client
            SignalRClient signalR = new SignalRClient();

            // Registers SignalR_TemperatureChanged() as an event handler method that runs, when the event "signalR.TemperatureChanged" is triggered 
            signalR.TemperatureChanged += SignalR_TemperatureChanged;
            // Registers SignalR_SensorStopped() as an event handler method that runs, when the event "signalR.SensorStopped" is triggered 
            signalR.SensorStopped += SignalR_SensorStopped;

            signalR.InitializeSignalR();
            // Gets the reference to the Hub proxy created in the SignalRClient
            hub = signalR.SignalRHub;
        }


        // An event handler that runs when a "TemperatureChanged" event is triggered in SignalRClient
        private async void SignalR_TemperatureChanged(object sender, TemperatureChangedEventArgs e)
        {
            await Dispatcher.RunAsync(
                // Changes the text on this client according to the new temperature data received
                Windows.UI.Core.CoreDispatcherPriority.Normal,
                () => temp.Text = (e.Temperature + "°C"));
        }


        // An event handler that runs when a "SensorStopped" event is triggered in SignalRClient
        private async void SignalR_SensorStopped(object sender, TemperatureChangedEventArgs e)
        {
            await Dispatcher.RunAsync(
                // Changes the text on this client according to the new temperature data received
                Windows.UI.Core.CoreDispatcherPriority.Normal,
                () => temp.Text = (e.Temperature));
        }


        // An event handler that runs when the "Start Sensor" button click event is triggered in MainPage by user
        private async void StartSensorClick(object sender, RoutedEventArgs e)
        {
            // Executes a method "newUpdate" on the server side hub asynchronously, with params of type Object[]
            await hub.Invoke("newUpdate", new object[] { "START_SENSOR", "Last msg from User Device: Requesting sensor to start.." });
        }


        // An event handler that runs when the "Stop Sensor" button click event is triggered in MainPage by user
        private async void StopSensorClick(object sender, RoutedEventArgs e)
        {
            // Executes a method "newUpdate" on the server side hub asynchronously, with params of type Object[]
            await hub.Invoke("newUpdate", new object[] { "STOP_SENSOR", "Last msg from User Device: Requesting sensor to stop.." });
        }
    }
}
