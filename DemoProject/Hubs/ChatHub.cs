using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System.Collections.Generic;
using DemoProject.Models;
using Microsoft.AspNetCore.Http;
using System.Web;
using System;
using System.Linq;

namespace DemoProject.Hubs
{
    public class ChatHub : Hub
    {
        static List<ConnectedAcc> ConnectedUsers = new List<ConnectedAcc>();


        //Create a new object to ConnectedUsers list and broadcast new user joined message/load online users list
        public async Task Connect(string user)
        {

            var id = Context.ConnectionId;
 
            if(ConnectedUsers.SingleOrDefault(x => x.Username == user)==null)
            {
                ConnectedUsers.Add(new ConnectedAcc { Username = user, ID = id });
                await Clients.AllExcept(id).SendAsync("Join", user);
                
            }
            await Clients.All.SendAsync("Online", ConnectedUsers);

        }
        //Sends chat message to all clients
        public async Task SendMessage(string user, string time, string message)
        {

            await Clients.All.SendAsync("ReceiveMessage", user, time, message, ConnectedUsers);
        }
  




        //On Disconnect removes username from list and sends disconnect msg to other clients.
        public override Task OnDisconnectedAsync(Exception exception)
        {

            var item = ConnectedUsers.SingleOrDefault(x => x.ID == Context.ConnectionId);
            if (item != null)
            {
                
                ConnectedUsers.Remove(item);
                Clients.All.SendAsync("Disconnected", item.Username);
                Clients.All.SendAsync("Online", ConnectedUsers);
            }

            return base.OnDisconnectedAsync(exception);
        }
    }
}