## What is this project about?
(coming soon)

## What do we need?
* Windows PC
* Visual Studios IDE
* Microsoft Azure account

* Raspberry Pi and accessories (micro-USB power cable, LAN cable or Wifi dongle)
* Breadboard and wires
* Temperature sensor (TMP36)
* ADC converter (MCP3208)


## What do we do?
1. Set up your local working space
2. Set up Raspberry Pi component (TemperatureMeasurement)
4. Set up SignalR web api on Microsoft Azure (SignalRIoT)
5. Set up User Device Client (TemperatureClient)


#### Step 1: Set up your local working space
* Download project folder to your local PC
* Either "Clone in Desktop" (if you have the GitHub desktop app), or "Download ZIP" (and extract the content manually)
![Download Project](/images/prepare-download_project.png)

#### Step 2a: Set up Raspberry Pi component (hardware)
![Hardware Overview](/images/raspi-hardware_overview.jpg)
![Hardware Detailed](/images/raspi-hardware_detailed.jpg)

#### Step 2b: Set up Raspberry Pi component (software)
![Change API URL](/images/raspi-change_api_url.png)
![Build Project](/images/raspi-build_project.png)
![RasPi Preview](/images/raspi-hardware_preview.jpg)

#### Step 4: Set up SignalR web api on Microsoft Azure
![Create Web App](/images/azure-create_app_1.png)
![Create Web App](/images/azure-create_app_2.png)
![Preview Web App](/images/azure-app_preview.png)

![Publish Web API](/images/signalr-publish_app_1.png)
![Publish Web API](/images/signalr-publish_app_2.png)
![Publish Web API](/images/signalr-publish_app_3.png)
![Publish Web API](/images/signalr-publish_app_4.png)

#### Step 5: Set up User Device Client
![Change API URL](/images/client-change_api_url.png)
![Build Project](/images/client-build_project.png)
![Preview Device Client](/images/client-app_preview.png)

## Credits
A big thank you to:
* Microsoft Singapore DX team for their contributions and support (Riza, Chun Siong, May, Kim)
* Other tutorial sites for their valuable resources
  * [ConnectTheDots](https://github.com/msopentech/connectthedots/)
  * [TMP36 Temperature Tutorial](https://plot.ly/raspberry-pi/tmp36-temperature-tutorial/)
