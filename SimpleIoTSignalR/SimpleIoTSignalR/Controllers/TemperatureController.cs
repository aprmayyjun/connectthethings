using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace SimpleIoTSignalR.Controllers
{
    [HubName("updater")]
    public class TemperatureHub : Hub
    {
        public void NewUpdate(string commandMessage, string state)
        {
            Clients.All.newUpdate(commandMessage, state);
        }
    }
}
