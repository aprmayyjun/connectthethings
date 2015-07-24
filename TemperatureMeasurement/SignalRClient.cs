using Microsoft.AspNet.SignalR.Client;
using System;
using System.Diagnostics;

namespace TemperatureMeasurement
{
    /*
     * This class controls the logic that initializes and manages the SignalRClient object tagged to this IoT client
     */
    public class SignalRClient
    {
        // A client side proxy for a server side hub
        private IHubProxy _hub;
        // Public getter to return reference to the SignalR hub proxy
        public IHubProxy SignalRHub { get { return _hub; } }

        // Declares an event that can be triggered, and is handled by a method of type "EventHandler<StateChangedEventArgs>"
        public event EventHandler<StateChangedEventArgs> StateChanged = delegate { };


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
                case "TEST_DATA":
                    Debug.WriteLine(state);
                    break;

                case "START_SENSOR":
                    // Triggers the StateChanged event
                    // false => sensor !isRunning (i.e. is not running) / state => "Requesting sensor to START!" (from TemperatureClient)
                    StateChanged(this, new StateChangedEventArgs(false, state));
                    break;

                case "STOP_SENSOR":
                    // Triggers the StateChanged event
                    // true => sensor isRunning / state => "Requesting sensor to STOP!" (from TemperatureClient)
                    StateChanged(this, new StateChangedEventArgs(true, state));
                    break;
            }
        }
    }
}
