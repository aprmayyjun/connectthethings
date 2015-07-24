using System;

namespace TemperatureMeasurement
{
    /*
     * This class controls the logic to store and manipulate the Event arguments to the StateChanged event
     */
    public class StateChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the state.
        /// </summary>
        public string State { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is running.
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StateChangedEventArgs"/> class.
        /// </summary>
        /// <param name="isRunning">if set to <c>true</c> [is running].</param>
        /// <param name="state">The state.</param>
        public StateChangedEventArgs(bool isRunning, string state)
        {
            this.IsRunning = isRunning;
            this.State = state;
        }
    }
}
