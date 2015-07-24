using System;

namespace TemperatureMeasurement
{
    /*
     * This class controls the logic to store and manipulate the Event arguments to the TemperatureChanged event
     */
    public class TemperatureChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the temperature.
        /// </summary>
        public string Temperature { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TemperatureChangedEventArgs"/> class.
        /// </summary>
        /// <param name="temperatue">The temperatue.</param>
        public TemperatureChangedEventArgs(string temperatue)
        {
            this.Temperature = temperatue;
        }
    }
}
