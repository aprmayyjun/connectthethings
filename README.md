## What is this project about?
(coming soon)

## Resources needed
* Windows PC
* Visual Studio IDE (2013 onwards)
* Microsoft Azure account (free trial or DreamSpark subscription will do)

__Hardware components:__
* Raspberry Pi 2 (Model B)
* Micro-USB power cable
* LAN cable or RasPi Wifi dongle
* Breadboard and wires
* Temperature sensor (TMP36)
* ADC converter (MCP3208)
* Monitor screen and HDMI cable (for RasPi)


## Steps
1. Set up your local working space
2. Set up Raspberry Pi component (TemperatureMeasurement)
4. Set up SignalR web API on Microsoft Azure (SimpleIoTSignalR)
5. Set up User Device Client (TemperatureClient)


#### Step 1: Set up your local working space
* Download project folder to your local PC
* Either "Clone in Desktop" (if you have the GitHub desktop app), or "Download ZIP" (and extract the content manually)
![Download Project](/images/prepare-download_project.png)

#### Step 2: Set up SignalR web API on Microsoft Azure
* Create a basic web app on your Azure portal. Log-in [here](manage.windowsazure.com)
![Create Web App](/images/azure-create_app_1.png)
![Create Web App](/images/azure-create_app_2.png)

* If successful, you should see the web page below when you navigate to the URL you specified
![Preview Web App](/images/azure-app_preview_1.png)

* Open the project solution __/SimpleIoTSignalR/SimpleIoTSignalR.sln__ in Visual Studio
* Right-click on the project name __SimpleIoTSignalR__ and select __Publish..__
![Publish Web API](/images/signalr-publish_app_1.png)
* Select __Profile__ on the left panel and then __Microsoft Azure Web Apps__
![Publish Web API](/images/signalr-publish_app_2.png)
* You should be prompted to log-in to your Azure account
* After successful log-in, select the web app you created earlier
![Publish Web API](/images/signalr-publish_app_3.png)
* Select __Publish__ without changing any settings on the dialog
![Publish Web API](/images/signalr-publish_app_4.png)

* If successful, you should see the new web page below when you navigate to the URL you specified
![Preview Web App](/images/azure-app_preview.png)

#### Step 3a: Set up Raspberry Pi component (hardware)
* Connect the hardware components as per pictured below
![Hardware Overview](/images/raspi-hardware_overview.jpg)
![Hardware Detailed](/images/raspi-hardware_detailed.jpg)

#### Step 3b: Set up Raspberry Pi component (software)
* Download and set-up Windows 10 IoT Core on your Raspberry Pi 2, instructions [here](http://ms-iot.github.io/content/en-US/win10/SetupPCRPI.htm)
* Connect your RasPi to the Internet, either via LAN cable (wired) or Wifi dongle (wireless)
	* Ensure that your RasPi and development PC are on the same network
	* To check the IP address of your RasPi, you need to connect it to a monitor screen

* Open the project solution __/TemperatureMeasurement/TemperatureMeasurement.sln__ in Visual Studio
* Open the file __SignalRClient.cs__ for edit
* Change the SignalR web API URL to the one you created earlier in Step 2, as per pictured below
![Change API URL](/images/raspi-change_api_url.png)

* Build the solution via the __Remote Machine__ option, as per pictured below
* Enter the IP address of your Raspberry Pi, select __None__ for Authentication Mode
![Build Project](/images/raspi-build_project.png)

* To check if the app is working correctly on your RasPi, connect it to the monitor and it should appear like this below
![RasPi Preview](/images/raspi-hardware_preview.jpg)

#### Step 4: Set up User Device Client
* Open the project solution __/TemperatureClient/TemperatureClient.sln__ in Visual Studio
* Open the file __SignalRClient.cs__ for edit
* Change the SignalR web API URL to the one you created earlier in Step 2, as per pictured below
![Change API URL](/images/client-change_api_url.png)

* Build the solution via the *Local Machine* option, as per pictured below
![Build Project](/images/client-build_project.png)

* A desktop app should run like this below
* This Windows Universal App client can run on any Windows 10 devices (e.g. Windows PC, mobile, Xbox)
![Preview Device Client](/images/client-app_preview.png)

## Credits
A big thank you to:
* Microsoft Singapore DX team for their contributions and support (Riza, Chun Siong, May, Kim)
* Other tutorial sites for their valuable resources
  * [ConnectTheDots](https://github.com/msopentech/connectthedots/)
  * [TMP36 Temperature Tutorial](https://plot.ly/raspberry-pi/tmp36-temperature-tutorial/)
