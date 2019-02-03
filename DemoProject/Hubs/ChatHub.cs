using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;


namespace DemoProject.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string time, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, time, message);
        }
    }
}