using Microsoft.AspNet.SignalR.Client;
using System;
using System.Diagnostics;

namespace TemperatureClient
{
    /*
     * This class controls the logic that initializes and manages the SignalRClient object tagged to this app client
     */
    public class SignalRClient
    {
        // A client side proxy for a server side hub
        private IHubProxy _hub;
        // Public getter to return reference to the SignalR hub proxy
        public IHubProxy SignalRHub { get { return _hub; } }

        // Declares an event that can be triggered, and is handled by a method of type "EventHandler<TemperatureChangedEventArgs>"
        public event EventHandler<TemperatureChangedEventArgs> TemperatureChanged = delegate { };
        // Declares an event that can be triggered, and is handled by a method of type "EventHandler"
        public event EventHandler<TemperatureChangedEventArgs> SensorStopped = delegate { };


        public async void InitializeSignalR()
        {
            // Establishes connection to the server side hub
            var hubConnection = new HubConnection("http://iot-webapi.azurewebsites.net/signalr");
            _hub = hubConnection.CreateHubProxy("updater");

            // Registers for an event with the specified name and callback
            _hub.On<string, string>("newUpdate", ProcessMessage);

            hubConnection.Received += (obj) => Debug.WriteLine("hubConnection_Received(): " + obj.ToString());
            hubConnection.StateChanged += (obj) =>
            {   // For debugging purposes
                Debug.WriteLine("\n\n");
                Debug.WriteLine("CLIENT Old State: " + obj.OldState.ToString());
                Debug.WriteLine("CLIENT New State: " + obj.NewState.ToString());
                Debug.WriteLine("\n\n");
            };

            await hubConnection.Start();

        }


        // Callback method registered on the hub, invoked when event "newUpdate" is triggered on server side hub
        private void ProcessMessage(string commandMessage, string state)
        {
            // Selects the event to trigger based on commandMessage received from hub
            switch (commandMessage)
            {
                case "TEMP_CHANGED":
                    // Triggers the TemperatureChanged event
                    TemperatureChanged(this, new TemperatureChangedEventArgs(state));
                    break;
                case "SENSOR_OFF":
                    // Triggers the SensorStopped event
                    SensorStopped(this, new TemperatureChangedEventArgs(state));
                    break;
            }
        }
    }
    

    /*
     * This class controls the logic to store and manipulate the Event arguments to the TemperatureChanged event
     */
    public class TemperatureChangedEventArgs : EventArgs
    {
        // Variable that stores the temperature data
        public string Temperature { get; private set; }

        // Sets the temperature data based on data received through argument
        public TemperatureChangedEventArgs(string temp)
        {
            this.Temperature = temp;
        }
    }

}
