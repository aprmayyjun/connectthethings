using Microsoft.AspNet.SignalR.Client;
using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace TemperatureMeasurement
{
    /// <summary>
    /// This class controls the logic behind the varoius components in MainPage's XAML file
    /// </summary>
    public sealed partial class MainPage : Page
    {
        // A client side proxy for a server side hub
        IHubProxy hub;
        SensorSource sensor = null;


        // This constructor is called when MainPage is initialized
        public MainPage()
        {
            this.InitializeComponent();

            // Registers MainPage_Loaded() as an event handler method that runs, when the event "this.Loaded" is triggered 
            this.Loaded += MainPage_Loaded;
            // Registers MainPage_Unloaded() as an event handler method that runs, when the event "this.Unloaded" is triggered 
            this.Unloaded += MainPage_Unloaded;
        }


        // An event handler that runs on successful page unload
        private void MainPage_Unloaded(object sender, RoutedEventArgs e)
        {
            if (sensor != null)
                sensor.Dispose();
        }


        // An event handler that runs on successful page load
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            // Prepares a SignalRClient object for this IoT client
            SignalRClient signalR = new SignalRClient();

            // Registers SignalR_StateChanged() as an event handler method that runs, when the event "signalR.StateChanged" is triggered 
            signalR.StateChanged += SignalR_StateChanged;

            signalR.InitializeSignalR();
            // Gets the reference to the Hub proxy created in the SignalRClient
            hub = signalR.SignalRHub;


            // Prepares a SensorSource object for this IoT client
            sensor = new SensorSource();

            // Registers Sensor_StateChanged() as an event handler method that runs, when the event "sensor.StateChanged" is triggered 
            sensor.StateChanged += Sensor_StateChanged;
            // Registers Sensor_ValueChanged() as an event handler method that runs, when the event "sensor.ValueChanged" is triggered
            sensor.ValueChanged += Sensor_ValueChanged;
        }


        // An event handler that runs when a "signalR.StateChanged" event is triggered in SignalRClient 
        // (result of "START_SENSOR" or "STOP_SENSOR" message from hub, i.e. other clients asked hub to start/stop sensor)
        private async void SignalR_StateChanged(object sender, StateChangedEventArgs e)
        {
            SignalRClient signalR = sender as SignalRClient;

            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                // If sensor is not already running (i.e. client requested to START_SENSOR)
                if (!e.IsRunning)
                {
                    // Turns the toggle switch to ON
                    toggleSwitch.IsOn = true;
                }
                // else if sensor is already running (i.e. client requested to STOP_SENSOR)
                else
                {
                    // Turns the toggle switch to OFF
                    toggleSwitch.IsOn = false;
                }

                // If the message received from hub is not empty
                if (!String.IsNullOrEmpty(e.State))
                {
                    // Sets the display text to the message received from hub
                    messageFromClient.Text = e.State;
                }
            });
        }


        // An event handler that runs when a "sensor.StateChanged" event is triggered in SensorSource
        private async void Sensor_StateChanged(object sender, StateChangedEventArgs e)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
            {
                // If the sensor is running, i.e. turned ON
                if (e.IsRunning)
                {
                    // Changes the display text on the XAML file to reflect that sensor is running
                    sensorStatus.Text = "Sensor Status: Running";
                    icon.Foreground = new SolidColorBrush(Colors.Black);
                }
                // else if the sensor is not running, i.e. turned OFF
                else
                {
                    // Changes the display text on the XAML file to reflect that sensor is not running
                    sensorStatus.Text = "Sensor Status: Stopped";
                    tbkValue.Text = "[n/a]";
                    icon.Foreground = new SolidColorBrush(Colors.Gray);
                    
                    if (hub == null) return;
                    try
                    {
                        // Executes a method "newUpdate" on the server side hub asynchronously, with params of type Object[]
                        // (notifies clients that sensor is turned off)
                        await hub.Invoke("newUpdate", new object[] { "SENSOR_OFF", "[n/a]" });
                    }
                    catch { }
                }
            });
        }


        // An event handler that runs when a "sensor.ValueChanged" event is triggered in SensorSource
        private async void Sensor_ValueChanged(object sender, TemperatureChangedEventArgs e)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
            {
                // Changes the value on MainPage to display the new temperature
                tbkValue.Text = String.Format("{0:F02} °C", e.Temperature);

                if (hub == null) return;
                try
                {
                    // Executes a method "newUpdate" on the server side hub asynchronously, with params of type Object[]
                    // (notifies clients that sensor has a temperature change)
                    await hub.Invoke("newUpdate", new object[] { "TEMP_CHANGED", e.Temperature });
                }
                catch { }

            });
        }


        // An event handler that runs when the "Toggle" switch click event is triggered in MainPage by user
        private void toggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            // If user is turning the toggle ON
            if (toggleSwitch.IsOn)
            {
                sensor.Start();

                // Code below currently not in use
                /*
                if (hub == null) return;
                try
                {
                    await hub.Invoke("newUpdate", new object[] { "SYSTEM", "START" });
                }
                catch { }
                */
            }
            // Else if user is turning the toggle OFF
            else
            {
                sensor.Stop();

                // Code below currently not in use
                /*
                if (hub == null) return;
                try
                {
                    await hub.Invoke("newUpdate", new object[] { "SYSTEM", "STOP" });
                }
                catch { }
                */
            }
        }
    }
}