using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.Spi;

namespace TemperatureMeasurement
{
    public class SensorSource : IDisposable
    {
        private bool isDisposed = false;
        private bool isRunning = false;
        private bool isError = false;

        // Based on the measurement from the board. By right it should be 5.0V, in actual this is not always table and fix
        private const double referenceVoltage = 4.58; 

        private SpiDevice device;

        byte[] readBuffer = new byte[3];
        byte[] writeBuffer = new byte[3] { 0x06, 0x00, 0x00 };

        // Declares 2 events that can be triggered, and are handled by methods of type "EventHandler<TemperatureChangedEventArgs>"
        // and type "EventHandler<StateChangedEventArgs>"
        public event EventHandler<TemperatureChangedEventArgs> ValueChanged = delegate { };
        public event EventHandler<StateChangedEventArgs> StateChanged = delegate { };
        

        // Initializes a new instance of the class
        public SensorSource()
        {
            Init();
        }

       
        // Sets up this instance of the SensorSource
        private async void Init()
        {
            await Task.Run(async () =>
            {
                try
                {
                    var settings = new SpiConnectionSettings(0);
                    settings.ClockFrequency = 500000;
                    settings.Mode = SpiMode.Mode0;

                    string spi = SpiDevice.GetDeviceSelector("SPI0");
                    var deviceInfo = await DeviceInformation.FindAllAsync(spi);
                    device = await SpiDevice.FromIdAsync(deviceInfo[0].Id, settings);

                    isError = false;
                }
                catch (Exception ex)
                {
                    isError = true;
                    // Triggers the StateChanged event
                    // false => sensor !isRunning (i.e. is not running due to error) / state => "INITIALIZE ERROR"
                    StateChanged(this, new StateChangedEventArgs(false, "INITIALIZE ERROR"));
                    Debug.WriteLine(ex.Message);
                }
            }).ContinueWith((task) => ReadData());
        }


        // Starts this sensor
        public void Start()
        {
            isRunning = true;
            ReadData();
        }


        // Stops this sensor
        public void Stop()
        {
            // Exits the "while (isRunning)" async loop called by ReadData(), to stop reading the sensor
            isRunning = false;

            // Triggers the StateChanged event
            // false => sensor !isRunning (i.e. is not running) / state => "SENSOR STOPPED"
            StateChanged(this, new StateChangedEventArgs(false, "SENSOR STOPPED"));
        }


        // Reads the data from the sensor in an async loop while sensor's turned on
        private async void ReadData()
        {
            await Task.Run(() =>
            {
                // To continuously read data until SensorSource.Stop() is called
                while (isRunning)
                {
                    try
                    {
                        device.TransferFullDuplex(writeBuffer, readBuffer);
                        int res = ConvertToInt(readBuffer);
                        var tempC = ((referenceVoltage * 100) * res) / 4096;

                        // Triggers the ValueChanged event
                        ValueChanged(this, new TemperatureChangedEventArgs(tempC.ToString("F02")));

                        // Triggers the StateChanged event
                        // true => sensor isRunning / state => "OKAY"
                        StateChanged(this, new StateChangedEventArgs(true, "OKAY"));

                        Task.Delay(500).Wait();
                        isError = false;
                    }
                    catch (Exception)
                    {
                        isError = true;
                        // Triggers the StateChanged event
                        // false => sensor !isRunning (i.e. is not running due to error) / state => "READ ERROR"
                        StateChanged(this, new StateChangedEventArgs(false, "READ ERROR"));
                    }
                }
            });
        }

        
        // Converts the data to int
        public int ConvertToInt(byte[] data)
        {
            int result = data[1] & 0x00F;
            result <<= 8;
            result += data[2];
            return result;
        }


        #region IDisposable Members
        // Releases unmanaged and - optionally - managed resources
        private void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                if (!isDisposed)
                {
                    Stop();
                }
            }
        }


        // Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
